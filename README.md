# Desktop Assignment - Geolocation Storage Application

This project is a desktop application developed to manage and store geolocation data based on IP addresses or URLs. Using the IPStack API, it retrieves geolocation data and supports basic CRUD (Create, Read, Update, Delete) operations on stored data.
![image](https://github.com/user-attachments/assets/8839323e-3a66-42bb-b526-09f8f300ce7e)

## Project Summary

The application was designed with the following core requirements:
- Store geolocation data (latitude, longitude, city, etc.) in a database based on IP address or URL.
- Support adding, deleting, and retrieving geolocation data.
- Be implemented in WPF and support database connectivity.
- Enable easy setup for testing on local machines.
- Function resiliently under various failure conditions (e.g., database connection issues, API outages).

## Technologies and Libraries

### Frameworks and Libraries Used:
- **.NET Version**: .NET 8
- **MVVM Framework**: Caliburn.Micro
- **Database**: SQLite with Entity Framework Core for Object-Relational Mapping
- **UI Framework**: Material Design
- **Testing Frameworks**: XUnit and Moq
- **Dependency Injection**: SimpleContainer (provided by Caliburn.Micro)

## Setup and Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Mileek/DesktopAssignment.git
