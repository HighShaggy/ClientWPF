# ClientWpf

WPF-приложение на C#(.NET Framework 4.7.2) для управления клиентами и заявками. Использует MVVM, Entity Framework Core и PostgreSQL.

---

## Структура

- **Core/Commands** — `RelayCommand` для MVVM.
- **Data** — контекст базы данных (`AppDbContext.cs`) и скрипты:
  - `schema.sql` — создаёт таблицы
  - `seed.sql` — заполняет тестовыми данными
- **Domain/Models** — сущности: `Client`, `Request`, `BusinessArea`, `RequestStatus`
- **Presentation/ViewModels** — логика для клиентов и заявок
- **Presentation/Views** — окна приложения
- **Services** — сервисы для работы с данными

---

