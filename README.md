# Folder Synchronizer

## Overview
`FolderSynchronizer` is a C# console application that synchronizes two folders: the **SourceFolder** and the **ReplicaFolder**. The program ensures that the **ReplicaFolder** is a one-way, full, and identical copy of the **SourceFolder**. Synchronization is performed periodically, with logs recorded in both the console and a specified log file.

---

## Prerequisites
- **.NET 6.0 SDK** or later installed on your system.
- Ensure that the `SourceFolder` and `ReplicaFolder` paths exist and are accessible.

---

## How to Compile and Run

### **Compile the Program**
1. Open the project in **Visual Studio** or any C# IDE.
2. Build the solution to generate the executable file (`FolderSync.exe`).

   - The output executable is located at:
     ```
     bin\Debug\net6.0\FolderSync.exe
     ```

### **Run the Program**
To run the program, use the following command in the terminal or console:

```bash
FolderSync.exe <SourceFolder> <ReplicaFolder> <LogFilePath> <SyncIntervalInSeconds>
