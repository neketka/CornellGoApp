### Summary (what did you accomplish?)
Example: This pr changes the model in BackendModel to account for the new group state. These changes have been integrated fully into the backend.

### Purpose
Example: The old group model did not account for the maximum possible number of members in a team, but this pr accomplishes that.

## What changes did you make?
- [x] Example: Changed GroupExtensions.cs to include new functionality for updating group state.
- [x] Example: Changed GroupHub.cs to allow frontend to access this new group state.

## What improvements can be made?
- [ ] Example: Add more documentation to GroupExtensions.cs
- [ ] Example: Make public API cleaner

## Any breaking changes or new dependencies?
* Example: Added a reference to Newtonsoft.Json in NuGet.
* Example: Frontend cannot accept the old API and will crash if it tries, needs to be rewired. Remove old API step by step.
* Example: Create a new method in IClientHub interface and created default implementation. Might cause merge conflict and needs to be implemented.

# What is your plan for testing? To what extent has it been exectuted?
* Example: Make sure that the new group functionality is implemented correctly.
* Example: Check with frontend team that these changes are correct and the API exposes enough functionality.
