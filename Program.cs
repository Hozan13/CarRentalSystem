using System;
using System.Collections.Generic;
using System.Linq;


namespace CarRentalSystem
{
    class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsRented { get; set; } = false;

        public Car(string brand, string model, decimal dailyRate)
        {
            Brand = brand;
            Model = model;
            DailyRate = dailyRate;
        }
        public override string ToString()
        {
            return $"{Brand} {Model} - Daily Rate: {DailyRate:C} - {(IsRented ? "Rented" : "Available")}";
        }
    }

    class Customer
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Customer(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{Name} - Phone: {PhoneNumber}";
        }
    }

    class Rental
    {
        public Customer Customer { get; set; }
        public Car Car { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Rental(Customer customer, Car car, DateTime startDate, DateTime endDate)
        {
            Customer = customer;
            Car = car;
            StartDate = startDate;
            EndDate = endDate;
        }
        public decimal CalculateTotal()
        {
            return (EndDate - StartDate).Days * Car.DailyRate;
        }
        public override string ToString()
        {
            return $"Customer: {Customer.Name}, Car: {Car.Brand} {Car.Model}, " +
                      $"Dates: {StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}, Total: {CalculateTotal():C}";
        }
    }
    class Program
    {
        static List<Car> cars = new List<Car>();
        static List<Customer> customers = new List<Customer>();
        static List<Rental> rentals = new List<Rental>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("--- Car Rental System ---");
                Console.WriteLine("1. Add Car");
                Console.WriteLine("2. Add Customer");
                Console.WriteLine("3. Rent a Car");
                Console.WriteLine("4. List Rentals");
                Console.WriteLine("5. Return a Car");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Choose an option:");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddCar();
                        break;
                    case "2":
                        AddCustomer();
                        break;
                    case "3":
                        RentCar();
                        break;
                    case "4":
                        ListRentals();
                        break;
                    case "5":
                        ReturnCar();
                        break;
                    case "6":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void AddCar()
        {
            Console.WriteLine("Enter car brand: ");
            string brand = Console.ReadLine();
            Console.WriteLine("Enter car model: ");
            string model = Console.ReadLine();
            Console.WriteLine("Enter daily rental rate: ");
            decimal rate;
            while (!decimal.TryParse(Console.ReadLine(), out rate))
            {
                Console.WriteLine("Invalid input. Please enter a valid daily rate: ");
            }
            cars.Add(new Car(brand, model, rate));
            Console.WriteLine("Car added successfully!");
        }
        static void AddCustomer()
        {
            Console.WriteLine("Enter a customer name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter customer phone number: ");
            string phone = Console.ReadLine();
            customers.Add(new Customer(name, phone));
            Console.WriteLine("Customer added successfully!");
        }

        static void RentCar()
        {
            if (cars.Count == 0 || customers.Count == 0)
            {
                Console.WriteLine("No cars or customers available for rental.");
                return;
            }
            Console.WriteLine("--- Available Cars ---");
            for (int i = 0; i < cars.Count; i++)
            {
                if (!cars[i].IsRented)
                {
                    Console.WriteLine($"{i + 1}.{cars[i]}");
                }
            }
            Console.WriteLine("Select a car (enter the number): ");
            int carIndex;
            while (!int.TryParse(Console.ReadLine(), out carIndex) || carIndex < 1 || carIndex > cars.Count || cars[carIndex - 1].IsRented)
            {
                Console.WriteLine("Invalid choice. Please select a valid car: ");
            }
            Console.WriteLine("--- Customers ---");
            for (int i = 0; i < customers.Count;i++)
            {
                Console.WriteLine($"{i + 1}. {customers[i]}");
            }
            Console.WriteLine("Select a customer (enter the number): ");
            int customerIndex;
            while (!int.TryParse(Console.ReadLine(), out customerIndex) || customerIndex < 1 || customerIndex > customers.Count)
            {
                Console.WriteLine("Invalid choice. Please select a valid customer:");
            }
            Console.WriteLine("Enter rental start date (yyyy-MM-dd): ");
            DateTime startDate;
            while (!DateTime.TryParse(Console.ReadLine(), out startDate) || startDate < DateTime.Today)
            {
                Console.WriteLine("Invalid date or date is in the past. Please enter a valid start date:");
            }

            Console.WriteLine("Enter rental end date (yyyy-MM-dd): ");
            DateTime endDate;
            while (!DateTime.TryParse(Console.ReadLine(), out endDate) || endDate <= startDate)
            {
                Console.WriteLine("Invalid date. Please enter a valid end date:");
            }

            var car = cars[carIndex - 1];
            var customer = customers[customerIndex - 1];

            bool isDateConflict = rentals.Any(r => r.Car == car && (startDate < r.EndDate && endDate > r.StartDate));
            if (isDateConflict)
            {

                var conflictingRental = rentals.First(r => r.Car == car && (startDate < r.EndDate && endDate > r.StartDate));
                Console.WriteLine($"This car is already booked during the selected dates. Conflicting rental: {conflictingRental}");


                return;
            }

            car.IsRented = true;
            rentals.Add(new Rental(customer, car, startDate, endDate));
            Console.WriteLine("Car rented successfully!");
        }
        
        static void ListRentals()
        {
            if (rentals.Count == 0)
            {
                Console.WriteLine("No rentals available.");
                return;
            }
            Console.WriteLine("--- Rental Details ---");
            foreach (var rental in rentals)
            {
                Console.WriteLine(rental);
            }
        }

        static void ReturnCar()
        {
            if (rentals.Count == 0)
            {
                Console.WriteLine("No rentals available for return.");
                return;
            }

            Console.WriteLine("--- Rentals ---");
            for (int i = 0; i < rentals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {rentals[i]}");
            }

            Console.WriteLine("Select a rental to return (enter the number): ");
            int rentalIndex;
            while (!int.TryParse(Console.ReadLine(), out rentalIndex) || rentalIndex < 1 || rentalIndex > rentals.Count)
            {
                Console.WriteLine("Invalid choice. Please select a valid rental:");
            }

            var rental = rentals[rentalIndex - 1];
            rental.Car.IsRented = false;
            rentals.RemoveAt(rentalIndex - 1);
            Console.WriteLine("Car returned successfully!");
        }

    }
}
