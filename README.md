# Next-API

Серверная часть веб-приложения, написанная на C#

# Запуск

Для запуска приложения, необходимо выполнить следующее:

- Склонировать репозиторий себе на ПК
- Запустить `Docker Desktop`
- В файлах `Program.cs`, `docker-compose.yml` и `appsettings.json` изменить указанный порт на любой свободный
- В терминале прописать команды:
  
  ```bash
  docker compose up -d
  cd nextAPI
  dotnet ef database update
  ```
- Запустить программу через любой `IDE`, поддерживающий язык программирования `C#`

## Описание

Приложение имитирует серверную часть социальной сети. Обрабатывает `REST` запросы, поступающие с [клиентской части приложения](https://github.com/vacherkasskiy/Next-Client)

Клиентская сторона написана с использованием таких языков и технологий как:
- C#
- .NET
- dotnet ef
- Docker
- PostgreSql

Архитектура: [Clean Architecture](https://www.c-sharpcorner.com/article/clean-architecture-in-asp-net-core-web-api/)  
Код-стайл: [Microsoft docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
