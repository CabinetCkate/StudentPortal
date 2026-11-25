# StudentPortal

## Жоба сипаттамасы

StudentPortal — студенттерді оқу мақсатында тіркеуге, тізімін көруге және Admin рөлімен басқаруға арналған ASP.NET Core MVC қолданбасы. ASP.NET Core Identity пайдаланушыны тіркеу, құпиясөзді тексеру, cookie арқылы кіру және `Admin`/`Student` рөлдерін басқару үшін қолданылады. Әдепкі дерекқор — жоба қалтасында жасалатын SQLite `studentportal.db` файлы.

## Қолданылған технологиялар

- .NET 8 / ASP.NET Core MVC, Razor Views
- ASP.NET Core Identity Entity Framework Core 8.0.22
- Entity Framework Core SQLite 8.0.22
- `Microsoft.EntityFrameworkCore.Design` 8.0.22
- SQLite — жергілікті файлдық дерекқор; жарияланатын құпия мән жоқ

## Алғышарттар

- .NET 8 SDK және .NET 8 runtime
- Git
- SQLite деректерін көру қажет болса, DB Browser for SQLite
- Миграция жасау үшін `dotnet-ef` жаһандық құралы

## Клондау және жобаны дайындау

Репозиторийдің `StudentPortal` қалтасында жоба дайын. Жаңа жобаны нөлден жасауға арналған командалар:

```powershell
dotnet new mvc --name StudentPortal --framework net8.0
cd StudentPortal
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.22
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.22
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.22
```

GitHub репозиторийін клондау үлгісі:

```powershell
git clone https://github.com/CabinetCkate/StudentPortal.git
cd StudentPortal
```

Жоба файлына пакеттер тіркелгендіктен, бұрыннан бар checkout үшін `dotnet restore` жеткілікті.

## User Secrets: бастапқы Admin

`SeedAdmin:Email` және `SeedAdmin:Password` баптаулары бастапқы Admin пайдаланушысын автоматты жасау үшін ғана қажет. Олар берілмесе, қолданба қалыпты іске қосылады, `Admin` және `Student` рөлдерін дайындайды, ал жаңа пайдаланушылар тіркеліп Student рөлін алады. Admin қажет болса, құпияларды репозиторийден тыс сақтап, төмендегі командаларды орындаңыз:

```powershell
dotnet user-secrets set "SeedAdmin:Email" "admin@example.edu" --project .\StudentPortal\StudentPortal.csproj
dotnet user-secrets set "SeedAdmin:Password" "[REDACTED]" --project .\StudentPortal\StudentPortal.csproj
```

Құпиясөз кемінде 8 таңбалы, бас әріп, кіші әріп, сан және арнайы таңбасы бар болсын. Өндірісте User Secrets орнына құпия басқару жүйесін немесе `SeedAdmin__Email` және `SeedAdmin__Password` environment variables қолданыңыз.
`appsettings.json` файлына жазылмайды. Осы checkout үшін `UserSecretsId` бұрыннан бапталған, сондықтан тек төмендегі екі команданы репозиторий түбірінен орындаңыз. Таза жаңа жобада ғана `dotnet user-secrets init --project .\StudentPortal\StudentPortal.csproj` командасын бір рет қолданыңыз. Құпиясөздің орнына өзіңіз ойлап тапқан құпия мәнді енгізіңіз:

```powershell
dotnet user-secrets set "SeedAdmin:Email" "admin@example.edu" --project .\StudentPortal.csproj
dotnet user-secrets set "SeedAdmin:Password" "ӨЗІҢІЗДІҢ_ҚҰПИЯСӨЗІҢІЗДІ_ОСЫ_ЖЕРГЕ_ЕНГІЗІҢІЗ" --project .\StudentPortal.csproj
```

Құпиясөз кемінде 8 таңбалы, бас әріп, кіші әріп, сан және арнайы таңбасы бар болсын. Өндірісте дәл осы конфигурация кілттерін құпия басқару жүйесінде немесе `SeedAdmin__Email` және `SeedAdmin__Password` environment variables ретінде беріңіз. User Secrets өндірістік құпия сақтау қоймасы емес.

Егер Visual Studio іске қосқанда `SeedAdmin:Email` және `SeedAdmin:Password` жоқ деген `InvalidOperationException` шықса, бұл екі мән жобаның User Secrets қоймасына енгізілмегенін білдіреді. Жоғарыдағы екі `set` командасын орындап, Visual Studio-да іске қосу жобасы ретінде `StudentPortal` таңдалғанын тексеріп, қолданбаны қайта іске қосыңыз. `dotnet user-secrets list` тексергенде құпия мәндер терминалда көрінуі мүмкін, сондықтан оның шығысын жарияламаңыз. Құпияларды `appsettings.json` ішіне қоспаңыз.

`appsettings.json` ішінде тек `Data Source=studentportal.db` сияқты құпиясыз жергілікті SQLite жолы бар. `.gitignore` дерекқор файлдарын, `bin/`, `obj/`, `appsettings.Development.json` және құпияларға арналған файлдарды елемейді.

## Миграцияны құру және қолдану

Жоба каталогынан ретімен орындаңыз. Егер `dotnet-ef` жаһандық құрал бұрыннан орнатылған болса, install командасын өткізіп, қалғандарын орындаңыз. Бұл жоба EF Core 8 пакеттерімен жасалған; осы репозиторийдің миграциясы орнатылған `dotnet-ef` 9.0.10 құралымен де сәтті жасалып тексерілді.

```powershell
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef migrations add InitialIdentity
dotnet ef database update
```

`Migrations/` каталогындағы миграция файлдарын репозиторийге қосыңыз. Қолданба іске қосылғанда да `MigrateAsync()` орындалады; `dotnet ef database update` командасы миграцияны алдын ала тексеріп, дерекқорды қолмен дайындауға мүмкіндік береді.

Миграциядан кейін DB Browser for SQLite-та `studentportal.db` файлын ашып, **Database Structure** ішінен `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, `Students` кестелерін тексеріңіз. SQL арқылы тексеру:

```sql
SELECT name
FROM sqlite_master
WHERE type = 'table'
  AND name IN ('AspNetUsers', 'AspNetRoles', 'AspNetUserRoles', 'Students');

SELECT u.Email, r.Name AS RoleName
FROM AspNetUsers AS u
JOIN AspNetUserRoles AS ur ON ur.UserId = u.Id
JOIN AspNetRoles AS r ON r.Id = ur.RoleId
ORDER BY u.Email;
```

## Іске қосу

Қаласаңыз, Admin құпияларын орнатқан соң:

```powershell
dotnet restore
dotnet run
```

Консольде көрсетілген HTTPS/HTTP URL-ийг ашыңыз. `app.UseAuthentication()` нь `UseRouting()`-тен кейін, `UseAuthorization()`-тан бұрын тұрады. Қолданба іске қосылғанда миграциялар орындалып, рөлдер және конфигурация берілсе бастапқы Admin құрылады.

## Әдепкі рөлдер және қауіпсіздік

- `Admin`: бастапқы Admin-ге беріледі; студенттерді қосу, өңдеу және жоюға құқылы.
- `Student`: ашық тіркеуден өткен пайдаланушыға тек сервердегі код арқылы беріледі.
- Рөл формадан, соның ішінде жасырын өрістен қабылданбайды.
- `UserManager` пайдаланушы жазбаларын және Identity құпиясөз ережелерін басқарады; құпиясөз `PasswordHasher` арқылы тұздалған хэш түрінде сақталып, бастапқы мәтін күйінде дерекқорға түспейді.
- `SignInManager` құпиясөзді тексеріп, аутентификация cookie-сін жасайды және жояды.
- `RoleManager` `Admin`/`Student` рөлдерінің бар-жоғын тексеріп, оларды бір рет құрады.
- `IdentityDbContext<ApplicationUser>` Identity кестелерін `Students` оқу кестесімен бірге EF Core арқылы басқарады.
- `[Authorize]` кіруді, ал `[Authorize(Roles = "Admin")]` әкімші рөлін сервер жағында міндеттейді; интерфейстегі батырманы жасыру мұны алмастырмайды.
- Барлық POST әрекеттер anti-forgery қорғанысымен жабылған. Кіру кезінде `returnUrl` тек `Url.IsLocalUrl` арқылы қабылданады.
- Студент жазбаларын қабылдау үшін view model қолданылғандықтан, дерекқор моделінің артық өрістерін POST арқылы өзгертуге болмайды.

## Тестілеу қадамдары

| Қадам | Күтілетін нәтиже |
|---|---|
| 1. Тіркелу бетін бос жіберіңіз немесе жарамсыз email/қысқа құпиясөз енгізіңіз. | Клиенттік және серверлік тексеру өтпейді; қазақша валидация қатесі шығып, пайдаланушы тіркелмейді. |
| 2. Бір email-пен екінші рет тіркеліңіз. | `Бұл email бұрыннан тіркелген` қатесі көрсетіледі; журналда тіркелу қатесі құпиясөзсіз жазылады. |
| 3. Жаңа email-пен тіркеліп, дерекқордағы `AspNetUserRoles` пен `AspNetRoles` кестесін тексеріңіз. | Жаңа пайдаланушы `Student` рөлімен байланысады; формада рөл өрісі болмайды. |
| 4. Шыққан күйде `/Students` мекенжайын ашыңыз. | `/Account/Login` бетіне `returnUrl` сақталып бағытталады. |
| 5. `Student` ретінде тізімге кіріп, `/Students/Delete/1` URL-ийг шумақпен ашыңыз. | Жою POST әдісімен қорғалған, жою бетінің GET-і Admin-мен шектелген; Student `AccessDenied` бетіне бағытталады. |
| 6. Бастапқы Admin ретінде кіріп, студенттер тізімін ашып, жоюды растаңыз. | Admin тізімді көреді; жазба POST сұрауымен жойылады. |
| 7. GET әдісімен `/Account/Logout` мекенжайын сұраңыз. | GET әрекеті анықталмағандықтан 405/404 шығады; тек antiforgery токені бар POST logout орындайды. |

Тікелей URL-ді тексеруде `{id}` орнына шынымен бар Student жазбасының ID-сін қолданыңыз. Алдын ала тест дерегі жоқ болса, Admin ретінде бір студент қосыңыз.

## Скриншоттар

Төмендегі скриншоттарды түсіріп, көрсетілген атаулармен `docs/screenshots/` ішіне сақтаңыз:

1. [Тіркелу формасы және валидация](docs/screenshots/01-registration.png)
2. [Сәтті кіру және Student тізімі](docs/screenshots/02-login-student-list.png)
3. [Student үшін AccessDenied шектеуі](docs/screenshots/03-student-access-denied.png)
4. [Admin рөлімен тізім және жою мүмкіндігі](docs/screenshots/04-admin-access.png)
5. Қосымша: [SQLite рөлдер кестесі](docs/screenshots/05-identity-roles-database.png)

Скриншотта пароль, құпиясөз өрісі немесе нақты пайдаланушының жеке деректері көрінбесін.

## Бақылау тестінің жауаптары

Сұрақ мәтіндері мен жауап нұсқалары берілмегендіктен, әр әріпке мазмұндық дәлел ойдан қосылмайды. Төмендегі кілт тапсырмада берілген ретімен сақталды; нақты түсіндірмесін тапсырарда сұрақ мәтінімен салыстырып тексеріңіз.

1. **Ә** — берілген жауап кілті бойынша.
2. **А** — берілген жауап кілті бойынша.
3. **Ә** — берілген жауап кілті бойынша.
4. **А** — берілген жауап кілті бойынша.
5. **А** — берілген жауап кілті бойынша.
6. **Ә** — берілген жауап кілті бойынша.
7. **Ә** — берілген жауап кілті бойынша.
8. **А** — берілген жауап кілті бойынша.
9. **А** — берілген жауап кілті бойынша.
10. **Ә** — берілген жауап кілті бойынша.

## Рефлексия

### 1. Неге рөлді жасырын өрістен қабылдауға болмайды?

Жасырын өріс браузерден келген кәдімгі HTTP дерегі, сондықтан пайдаланушы оны құралдар арқылы өзгерте алады. Егер сервер сол мәнге сенсе, шабуылдаушы өзін `Admin` деп белгілеп, рұқсатын көтеруі мүмкін. Бұл privilege escalation осалдығына әкеледі. Сондықтан тіркелу әрекеті `Student` рөлін сервер жағында ғана тағайындайды. Admin рөлі тек бастапқы seed конфигурациясы арқылы беріледі.

### 2. Delete батырмасын жасыру серверлік авторизацияны алмастыра ма?

Жоқ, батырманы жасыру интерфейсті ыңғайлы етеді, бірақ сұрауды қорғамайды. Пайдаланушы URL-ді тікелей шақыра алады немесе қолмен POST сұрауын жібере алады. Клиент жағындағы тексерулерді айналып өту оңай. Сондықтан жоюдың GET және POST әрекеттеріне `[Authorize(Roles = "Admin")]` серверлік атрибуты қойылған. POST-та бұған қоса antiforgery токені талап етіледі.

### 3. Тіркелу қатесін журналдан және ModelState-тен қалай тексердім?

Алдымен `ModelState.Values.SelectMany(e => e.Errors)` арқылы модель валидациясының қателерін жинап, `ValidationSummary` және өріс деңгейіндегі хабарламаларда көрсетемін. Identity пайдаланушыны құруда сәтсіз болса, `result.Errors` элементтерін `ModelState.AddModelError` арқылы формаға қосамын. Қайталанған email үшін қазақша түсінікті хабарлама беремін. `ILogger` валидация сәтсіздігін және Identity қате кодтарын жазады, бірақ құпиясөзді немесе формадағы пароль мәнін журналға шығармайды.

## Қорытынды және бағалау критерийлері бойынша өзін-өзі тексеру

Орындау кезінде ескерілген маңызды мәселе — авторизация cookie-сі тек `UseAuthentication()` middleware-і `UseAuthorization()`-тан бұрын орындалғанда ғана сұрауға қосылады; әйтпесе рөлді тексеру дұрыс жұмыс істемейді. Бұл жобада middleware реті `UseRouting()` → `UseAuthentication()` → `UseAuthorization()` деп бекітілген. Identity құпиясөзді хэшпен сақтайды, ал бастапқы Admin құпиялары User Secrets-тен оқылады.

- [ ] 1. .NET 8 MVC құрылымы, SQLite және Identity package-тері бар.
- [ ] 2. Қазақша тексеруі бар тіркелу, сервер беретін Student рөлі және қайталанған email қатесі бар.
- [ ] 3. Кіру/шығу antiforgery-мен қорғалған; open redirect тексерілген.
- [ ] 4. Students тізімі кірушілерге ашық, Create/Edit/Delete сервер жағында Admin-мен шектелген.
- [ ] 5. Құпиялар репозиторийге жазылмайды; Identity құпиясөзді хэшпен сақтайды.
- [ ] 6. Миграциялар репозиторийде бар және Identity кестелері SQLite-та тексерілген.
- [ ] 7. Жеті сценарий орындалған, төрт негізгі скриншот пен GitHub репозиторий сілтемесі тіркелген.

GitHub репозиторийі: [CabinetCkate/StudentPortal](https://github.com/CabinetCkate/StudentPortal). Тапсыру алдында осы жобаның негізгі тармаққа жіберілгенін тексеріңіз.
