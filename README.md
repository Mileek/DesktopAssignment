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

   Build the Solution: Open the solution in Visual Studio and build the project.

2. **Database Setup: **
The application is configured to use SQLite, so no separate database setup is required. However, if testing under different database conditions, you may need to adjust the connection strings.

3. **Run the Application**:
 The application can be run directly from Visual Studio or from the generated executable. Test data can be loaded by interacting with the application to add sample geolocation entries.

## Potential Improvements
- **Logging**: A logging system (e.g., NLog) could enhance error tracking and application diagnostics.
- **Validation**: Regex or a dedicated validation framework could be used to enforce stricter input validation.
- **Localization**: Implementing a translation manager would allow for a more user-friendly, localized UI experience.
- **Dependency Injection**: Using a more advanced DI framework, such as AutoFac, could improve code scalability and maintainability.
- **Branch Management**: The project could benefit from using feature branches and pull requests to ensure smoother code review and collaboration.
## Notes
The application was developed as part of an assignment to demonstrate the ability to create a desktop application with CRUD functionality for geolocation data. Please note that the project is provided as-is and is meant for **testing** and **demonstration purposes**.

**Thank you for reviewing the project!**

## License
This project is open-source and available under the MIT License. Feel free to use, modify, and distribute it as per the license terms.
