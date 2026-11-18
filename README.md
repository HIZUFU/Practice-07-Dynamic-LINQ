# Практическая работа №7. Динамические LINQ-запросы

Подключена System.Linq.Dynamic.Core. В каталоге можно выбрать поле и условие поиска, участок, поле сортировки и направление. Where и OrderBy выполняются в запросе к SQLite перед проекцией Mapster. Поля ограничены списком, значение поиска передаётся параметром; неверные запросы возвращают 400. Сохранены мониторинг, журнал замеров, планировщик, маппинг и авторизация.

Сохранены каталог участков и оборудования, REST API, MVC-клиент, SQLite, Swagger, регистрация, вход и роли User/Admin.

Нужен .NET 10 SDK. В первом терминале:

```powershell
dotnet restore
$env:Admin__Password = 'StudyAdmin123!'
dotnet run --project ProductCatalogApp.Api --launch-profile http
```

Во втором терминале:

```powershell
dotnet run --project ProductCatalogApp.Client --launch-profile http
```

Клиент: http://localhost:5034. Swagger: http://localhost:5100/swagger. Администратор: admin, пароль из команды выше. Зарегистрированные пользователи получают роль User.
Мониторинг: http://localhost:5034/Monitoring.
Задачи: http://localhost:5034/Jobs (Admin). Hangfire Dashboard: http://localhost:5100/hangfire (локальный доступ).

