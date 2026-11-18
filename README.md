# Практическая работа №6. Планировщик задач

Для фоновых задач подключён Hangfire. Очереди и расписания хранятся в SQLite: сводка каждую минуту, очистка замеров старше 7 дней каждые 12 часов, разовая сводка и отложенная через 15 секунд. Результаты сохраняются в журнале БД. Страница задач и API управления доступны Admin; время расписаний — UTC.

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

