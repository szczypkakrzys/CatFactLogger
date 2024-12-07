# CatFactLogger Console Application

This is a simple .NET Console application that fetches random cat facts from an API and stores them in a specified file. The app is designed to be simple, extensible, and modular, with a focus on clear separation of concerns.

## Requirements

- **.NET 9 SDK**
  This project was developed using .NET 9, so to build and run it, you’ll need the .NET 9 SDK installed on your system. You can download it from the official [Microsoft .NET website](https://dotnet.microsoft.com/download).


## How to Run From Console
1. Clone or download the repository to your local machine.
2. Open the terminal and navigate to the project directory.
3. Run the following command to build the application:
   ```bash
   dotnet build
   ```
4. To run the application, use the following command:
    ```bash
    dotnet run -path="path/to/outputfile.txt"
    ```
    The application will fetch cat facts and store them in the specified file. If no path argument is provided, it will default to storing the facts in a file called CatFacts.txt in the current directory.

## Application Overview

The application interacts with the user via console input, providing an easy way to fetch cat facts and store them in a file.


### Logging

The application uses structured logging via `ILogger<T>` from `Microsoft.Extensions.Logging`, and logs are written to the console. Error logs are generated for failures, such as issues with making HTTP requests or writing to files. The logging system is configurable, with the minimum log level set to `LogLevel.Error`, ensuring that only critical errors are displayed on the console.


### Architecture

The core components are designed to be simple and focused on their respective tasks to maintain ease of use and future extensibility. The application follows a **simple architecture** using the **services pattern**. Given the scope of the task, a more complex layered architecture would be considered an **overkill**.

## How It Works

1. **Start the Application**: When the application is started, it looks for the `-path` argument in the command line input. If no path is provided, it defaults to `CatFacts.txt`.

2. **Fetch a Cat Fact**: The application makes an HTTP request to [https://catfact.ninja/fact](https://catfact.ninja/fact) and retrieves a random cat fact in JSON format.

3. **Save the Cat Fact**: The retrieved cat fact is then appended to the file specified by the user.

4. **Continue or Stop**: The application prompts the user to type `"cat"` if they want more facts. If the user enters something else, the application stops.

### Example Output

```bash
Hi!
Your Cat Facts will be stored in file: CatFacts.txt
Cat Fact: Baking chocolate is the most dangerous chocolate to your cat.
For more cat facts, type "cat": cat
Cat Fact: Cats have 3 eyelids.
For more cat facts, type "cat": no
Thanks! Come back for more Cat Fact :)
```
### Example File content
```json
{"fact": "Baking chocolate is the most dangerous chocolate to your cat.", "length": 61}
{"fact":"Cats have 3 eyelids.","length":20}
```