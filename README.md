# TestingToolkit

This repository is divided into four projects:

- **ConsoleUI**
- **StringProcessor**
- **UserProcessor**
- **TradeApiCaller**

## ConsoleUI

The `ConsoleUI` project serves as a simple user interface for the app.  
Users can navigate through the menu using **arrow keys** and confirm their actions with **Enter**.

> **Note:**  
> We are planning to migrate the UI to **WPF** in the future.

---

## StringProcessor

The `StringProcessor` project includes two main features:

- **StringGenerator**  
  Generates a string of a given length based on user input.  
  If the input is not a valid integer, a **character counter** feature is automatically applied.

- **StringMutator**  
  Takes a string value (e.g., `123 321 435 435 321`) and applies user-selected transformations:
  - `ApplyDelimiter`
  - `ApplySingleQuotes`
  - `ApplyDoubleQuotes`
  - `RemoveDuplicates`
  
  > Multiple actions can be applied at once.

  Result may look like e.g. `'123', '321', '435'`)

> **Planned Enhancements:**  
> New features will be added to `StringMutator` in the future.

---

## UserProcessor

The `UserProcessor` project handles **user registration** for an **IdentityServer**.  
These users can later be used for registration to the **AlzaTrade portal**.

Available features:
- **Generate random user login**
- **Generate custom user login**

---

## TradeApiCaller

The `TradeApiCaller` project includes two features:

- **Retrieving Registration Number**  
  Retrieves a free registration number of a specific type selected by the user.  
  The method ensures the number is **available** (not yet registered) before returning it.

- **HashId Resolver**  
  Converts user input to either an **Id** or a **HashId**, depending on the input type.

---

## Future Plans

- Migrate UI to **WPF**.
- Add new string processing features.
- Expand Trade API functionality.
