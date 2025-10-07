# GameStore Web Application

GameStore is a web-based platform for managing and browsing games, built with ASP.NET MVC targeting .NET Framework 4.6.1. It provides features for game catalog management, user comments, moderation, and platform support.

## Features

- **Game Management**: Add, edit, and view games with details such as name, key, price, units in stock, and platform support.
- **Comment System**: Users can add comments, reply to others, quote previous comments, and moderators can delete or ban users.
- **Moderation Tools**: Role-based access for moderators to manage comments and user bans.
- **Localization**: Uses resource files for multi-language support.
- **User Roles**: Supports user roles including regular users and moderators.

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET Framework 4.6.1

## Project Structure

- `GameStore.Web`: Main ASP.NET MVC web project.
- `GameStore.Web.Models`: ViewModels and domain models.
- `GameStore.Web.App_LocalResources`: Resource files for localization.
- `GameStore.Web.Tests`, `GameStore.Bll.Tests`: Unit test projects.

## Usage

- Browse games, view details, and manage inventory.
- Add comments to games, reply or quote other users.
- Moderators can delete comments and ban users.

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests.

## License

This project is licensed under the MIT License.