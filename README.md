# Dotnet-Template

## .NET Versions
Libraries in this repository are .NET 8 targetted.

## Building the Project
Before you can run the application, you'll need to build the project. Follow the steps below:

### Prerequisites
Make sure you have the following prerequisites installed on your machine:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.x)

Add SPIN's GitHub Nuget registry:
1. Create a GitHub classic token with read:packages enabled.
2. Use the following command to add SPIN's Nuget registry as "SPIN-github":
```bash
dotnet nuget add source https://nuget.pkg.github.com/SPIN-Space-Innovation/index.json -n SPIN-github -u SPIN-Space-Innovation --store-password-in-clear-text -p ${GITHUB_TOKEN}
```

### Clone the Repository
```bash
git clone https://github.com/SPIN-Space-Innovation/Dotnet-Template.git
cd Dotnet-Template
```

### Restore the Project dependencies
Use the following command to restore the project depedencies:
```bash
dotnet restore
```

### Build the Project
Use the following command to build the project:
```bash
dotnet build --no-restore
```

## License
Dotnet-Template is licensed under the [MIT license](LICENSE).
