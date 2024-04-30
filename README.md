## ASP.NET MVC Web API Todo List Project
This project is a basic ASP.NET MVC Web API application that allows users to manage their to-do lists. It includes functionalities for both regular users and administrators.

## Features:

* **User Management (Admin Only):**
   * Add new users
   * Get a list of all users
   * Delete a user (including their to-do items)
   
* **To-Do List Management:**
   * Users can view, add, update, and delete their own to-do items
   * Users cannot access other users' to-do items
* **Authentication:**
   * Login with username and password (JWT based)
   * Authorization for admin-restricted actions
     
## Server-Side Notes:
* **Data Persistence:**
   * Currently, to-do items and users are saved in JSON files for simplicity.
   * The code is designed to be easily switched to a database by injecting a service through an interface.
* **Logging:** 
   * Each request logs details including:
      * Start date & time
      * Controller & action name
      * Logged-in user (if any)
      * Operation duration
* **Authorization:**
   * Only administrators can add/delete users.
   * Users can only manage their own to-do items.
     
## Client-Side Notes:
* **Default Page:**
   * Shows the user's to-do list
   * Provides options to add, update, and delete items
* **Login:**
   * Redirects to login page if no valid token exists
* **Navigation:**
   * Admins can switch between to-do list and user list pages
* **Future Enhancements:**
   * User details update (name, password)
   * Admin access to own to-do list (watch and edit)
     
## Installation and Running the Project:
1. Ensure you have Visual Studio with ASP.NET MVC development capabilities installed.
2. Clone or download the project repository.
3. Open the project solution file (.sln) in Visual Studio.
4. Configure connection strings and file paths if needed (currently uses JSON files).
5. Run the project (F5).
   
## Technologies Used:
* ASP.NET MVC
* Web API
* JSON (data persistence - temporary)
* JWT (authentication)
