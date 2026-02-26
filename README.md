# Entity Framework Core — Guide Pédagogique Pas à Pas

> Projet de démonstration .NET 8 / EF Core avec SQL Server LocalDB.

---

## Table des matières

1. [Introduction à Entity Framework Core](#1-introduction-à-entity-framework-core)
2. [Prérequis et création du projet](#2-prérequis-et-création-du-projet)
3. [Installation des packages NuGet](#3-installation-des-packages-nuget)
4. [Créer les entités (Modèle)](#4-créer-les-entités-modèle)
5. [Créer le DbContext](#5-créer-le-dbcontext)
6. [Configurer les entités avec Fluent API](#6-configurer-les-entités-avec-fluent-api)
7. [Les Migrations](#7-les-migrations)
8. [Le Seed Data](#8-le-seed-data)
9. [Les opérations CRUD](#9-les-opérations-crud)
10. [Les Relations entre entités](#10-les-relations-entre-entités)
11. [Le point d'entrée et le menu console](#11-le-point-dentrée-et-le-menu-console)
12. [Récapitulatif de l'architecture](#12-récapitulatif-de-larchitecture)

---

## 1. Introduction à Entity Framework Core

### Qu'est-ce qu'un ORM ?

Un **ORM** (Object-Relational Mapper) est un outil qui fait le pont entre le monde **objet** (C#) et le monde **relationnel** (SQL). Au lieu d'écrire des requêtes SQL à la main, on manipule des objets C# et l'ORM se charge de traduire nos opérations en SQL.

### Qu'est-ce qu'Entity Framework Core ?

**Entity Framework Core** (EF Core) est l'ORM officiel de Microsoft pour .NET. Il permet de :

- **Mapper** des classes C# vers des tables SQL (et inversement).
- **Générer** le schéma de la base de données à partir du code (approche *Code First*).
- **Requêter** la base de données avec LINQ au lieu de SQL brut.
- **Suivre les modifications** (Change Tracking) et générer automatiquement les `INSERT`, `UPDATE`, `DELETE`.

### Approche Code First vs Database First

| Approche | Principe |
|----------|----------|
| **Code First** | On écrit les classes C# d'abord, puis EF Core génère la base de données via les migrations. C'est l'approche utilisée dans ce projet. |
| **Database First** | La base de données existe déjà, et on génère les classes C# à partir du schéma existant. |

---

## 2. Prérequis et création du projet

### Prérequis

- **.NET 8 SDK** installé
- **SQL Server LocalDB** (inclus avec Visual Studio)
- **Visual Studio 2022** (ou un éditeur + CLI .NET)

### Création du projet console

```bash
dotnet new console -n DemoEFCore
```

Le fichier `DemoEFCore.csproj` cible .NET 8 :

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

---

## 3. Installation des packages NuGet

Deux packages sont nécessaires :

```bash
# Le provider SQL Server pour EF Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Les outils pour les migrations (CLI / Package Manager Console)
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Théorie — Les Providers :**
EF Core est agnostique vis-à-vis de la base de données. Le *provider* (ici `SqlServer`) est le composant qui traduit les opérations EF Core en SQL spécifique au SGBD cible. Il existe des providers pour PostgreSQL (`Npgsql`), SQLite, MySQL, etc.

---

## 4. Créer les entités (Modèle)

### Théorie — Qu'est-ce qu'une entité ?

Une **entité** est une classe C# qui représente une **table** dans la base de données. Chaque **propriété** de la classe correspond à une **colonne** de la table. Chaque **instance** de la classe correspond à une **ligne** (un enregistrement).

### Les entités du projet

On place les entités dans un dossier `Entities/`.

**`Entities/Film.cs`** — Représente la table des films :

```csharp
namespace DemoEFCore.Entities;

public class Film
{
    public Guid Id { get; set; }            // Clé primaire
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public DateTime? CreatedAt { get; set; } // Nullable → valeur par défaut gérée côté SQL
}
```

**`Entities/Director.cs`** — Représente la table des réalisateurs :

```csharp
namespace DemoEFCore.Entities;

public class Director
{
    public Guid Id { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;
}
```

**`Entities/User.cs`** — Représente la table des utilisateurs :

```csharp
namespace DemoEFCore.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Lastname { get; set; }   // Nullable → colonne optionnelle
    public string? Firstname { get; set; }
}
```

### Points clés à retenir

- **`= null!`** : Indique au compilateur que la propriété sera initialisée par EF Core (évite les warnings `nullable`), tout en gardant la propriété *required* en base.
- **`string?`** (avec `?`) : La propriété est nullable en C# **et** en base de données.
- **`DateTime?`** : Le `?` rend le type valeur nullable. Ici, `CreatedAt` est nullable côté C# car sa valeur par défaut est générée côté SQL Server via `GETDATE()`. On n'a pas besoin de la fournir à la création d'un objet `Film`.
- **`Guid` comme clé primaire** : EF Core génère automatiquement un GUID si la propriété s'appelle `Id` ou `<NomClasse>Id` (convention).

---

## 5. Créer le DbContext

### Théorie — Le DbContext

Le `DbContext` est la **classe centrale** d'EF Core. Il représente une **session** avec la base de données et offre :

- Les **`DbSet<T>`** : chaque `DbSet` correspond à une table et permet de requêter/modifier les données.
- La **configuration de la connexion** (`OnConfiguring`).
- La **configuration du modèle** (`OnModelCreating`).

### Implémentation

```csharp
using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCore;

public class DataContext : DbContext
{
    // Chaque DbSet = une table SQL exposée comme collection C#
    public DbSet<Director> Directors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<User> Users { get; set; }

    // Configuration de la connexion à la base de données
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString =
            "Data Source=(localdb)\\mssqllocaldb;" +
            "Initial Catalog=FilmDb;" +
            "Integrated Security=True;" +
            "Trust Server Certificate=True";

        optionsBuilder.UseSqlServer(connectionString);
    }

    // Configuration du modèle (mapping entités ↔ tables)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Charge automatiquement toutes les classes IEntityTypeConfiguration
        // présentes dans l'assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
```

### Décryptage de la chaîne de connexion

| Paramètre | Rôle |
|-----------|------|
| `Data Source=(localdb)\mssqllocaldb` | Utilise l'instance LocalDB installée avec Visual Studio |
| `Initial Catalog=FilmDb` | Nom de la base de données (sera créée automatiquement) |
| `Integrated Security=True` | Authentification Windows (pas de login/mot de passe) |
| `Trust Server Certificate=True` | Accepte le certificat SSL du serveur local |

### Théorie — `ApplyConfigurationsFromAssembly`

Plutôt que d'enregistrer chaque configuration manuellement (`modelBuilder.ApplyConfiguration(new FilmConfiguration())`), la méthode `ApplyConfigurationsFromAssembly` **scanne automatiquement** l'assembly pour trouver toutes les classes qui implémentent `IEntityTypeConfiguration<T>` et les applique. C'est plus maintenable : il suffit d'ajouter une nouvelle classe de configuration, sans toucher au `DbContext`.

---

## 6. Configurer les entités avec Fluent API

### Théorie — Trois approches de configuration

EF Core propose trois façons de configurer le mapping entités ↔ tables :

| Approche | Principe | Exemple |
|----------|----------|---------|
| **Convention** | EF Core applique des règles par défaut (une propriété `Id` devient clé primaire, etc.) | Automatique |
| **Data Annotations** | Attributs placés directement sur les propriétés (`[Required]`, `[MaxLength(100)]`, etc.) | `[MaxLength(100)] public string Title { get; set; }` |
| **Fluent API** | Configuration dans des classes séparées via le `ModelBuilder`. C'est l'approche **la plus puissante** et celle utilisée dans ce projet. | Voir ci-dessous |

### Structure d'une configuration Fluent API

Chaque configuration implémente `IEntityTypeConfiguration<T>` et se place dans un dossier `Configurations/`.

**`Configurations/FilmConfiguration.cs`** :

```csharp
using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCore.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        // 1. Nom de la table + contrainte CHECK
        builder.ToTable("Films", schema =>
        {
            schema.HasCheckConstraint(
                "CK_Film_ReleasedYear_After_1500",
                "ReleasedYear >= 1500"
            );
        });

        // 2. Clé primaire
        builder.HasKey(f => f.Id);

        // 3. Configuration des colonnes
        builder.Property(f => f.Title)
            .IsRequired()           // NOT NULL
            .HasMaxLength(100);     // NVARCHAR(100)

        builder.Property(f => f.ReleasedYear)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasDefaultValueSql("GETDATE()");  // Valeur par défaut côté SQL
    }
}
```

**`Configurations/DirectorConfiguration.cs`** :

```csharp
public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("Directors");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Lastname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Firstname)
            .IsRequired()
            .HasMaxLength(50);
    }
}
```

**`Configurations/UserConfiguration.cs`** :

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", schema =>
        {
            schema.HasCheckConstraint(
                "CK_Users_Email_Format",
                "Email LIKE '_%@_%._%'"   // Validation basique du format email
            );
        });

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(u => u.Lastname).HasMaxLength(50);
        builder.Property(u => u.Firstname).HasMaxLength(50);
    }
}
```

### Récapitulatif des méthodes Fluent API utilisées

| Méthode | Effet en SQL |
|---------|-------------|
| `ToTable("Nom")` | Définit le nom de la table |
| `HasKey(x => x.Id)` | Définit la clé primaire |
| `IsRequired()` | Colonne `NOT NULL` |
| `HasMaxLength(n)` | `NVARCHAR(n)` au lieu de `NVARCHAR(MAX)` |
| `HasDefaultValueSql("...")` | Valeur par défaut calculée côté serveur |
| `HasCheckConstraint(...)` | Ajoute une contrainte `CHECK` sur la table |

---

## 7. Les Migrations

### Théorie — Qu'est-ce qu'une migration ?

Une **migration** est un fichier C# généré par EF Core qui décrit les **modifications à appliquer** au schéma de la base de données. Elle contient deux méthodes :

- **`Up()`** : Applique les changements (créer une table, ajouter une colonne…).
- **`Down()`** : Annule les changements (supprimer la table, retirer la colonne…).

Les migrations permettent de **versionner** le schéma de la base de données, exactement comme Git versionne le code.

### Commandes essentielles

```bash
# Créer une migration (génère les fichiers dans le dossier Migrations/)
dotnet ef migrations add NomDeLaMigration       # CLI .NET
Add-Migration NomDeLaMigration                  # Package Manager Console (Visual Studio)

# Appliquer les migrations en attente à la base de données
dotnet ef database update                       # CLI .NET
Update-Database                                 # Package Manager Console

# Annuler la dernière migration (si elle n'a pas été appliquée)
dotnet ef migrations remove                     # CLI .NET
Remove-Migration                                # Package Manager Console

# Revenir à une migration spécifique
dotnet ef database update NomDeLaMigration      # CLI .NET
Update-Database NomDeLaMigration                # Package Manager Console
```

### Migrations du projet

**Migration 1 — `InitialMigration`** : Crée les trois tables `Directors`, `Films` et `Users` avec leurs colonnes de base (sans configurations, types par défaut `NVARCHAR(MAX)`, pas de contraintes).

```
dotnet ef migrations add InitialMigration
dotnet ef database update
```

**Migration 2 — `AddConfigurations`** : Applique toutes les configurations Fluent API et le seed data. Cette migration contient beaucoup de changements car les configurations n'existaient pas encore lors de la première migration :

- **`Directors`** : `Lastname` et `Firstname` passent de `NVARCHAR(MAX)` à `NVARCHAR(50)`
- **`Films`** : `Title` passe à `NVARCHAR(100)`, ajout de la colonne `CreatedAt` avec valeur par défaut `GETDATE()`, ajout de la contrainte `CHECK` sur `ReleasedYear`, insertion des 20 films (seed data)
- **`Users`** : `Email` passe à `NVARCHAR(250)`, `Lastname`/`Firstname` à `NVARCHAR(50)`, ajout de la contrainte `CHECK` sur le format email

```
dotnet ef migrations add AddConfigurations
dotnet ef database update
```

### Fichiers générés par une migration

Chaque migration produit **deux fichiers** et met à jour le snapshot :

| Fichier | Rôle |
|---------|------|
| `YYYYMMDD_NomMigration.cs` | Le code `Up()` / `Down()` qui décrit les changements |
| `YYYYMMDD_NomMigration.Designer.cs` | Snapshot du modèle complet **au moment de cette migration** |
| `DataContextModelSnapshot.cs` | Snapshot du modèle **actuel** (mis à jour à chaque nouvelle migration) |

---

## 8. Le Seed Data

### Théorie — Qu'est-ce que le Seed Data ?

Le **Seed Data** (données d'amorçage) permet de pré-remplir la base de données avec des données initiales. C'est utile pour :

- Avoir des données de test dès la création de la base.
- Insérer des données de référence (pays, catégories, rôles…).

Le Seed Data est défini dans la configuration Fluent API avec `HasData()` et sera inclus dans une migration.

### Exemple dans `FilmConfiguration`

```csharp
builder.HasData(
    new Film {
        Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000001"),
        Title = "The Shawshank Redemption",
        ReleasedYear = 1994,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    },
    new Film {
        Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000002"),
        Title = "The Godfather",
        ReleasedYear = 1972,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    }
    // ... 20 films au total
);
```

### Points importants sur le Seed Data

- **L'`Id` doit être explicitement défini** : EF Core ne génère pas automatiquement les clés pour le Seed Data.
- **Les données sont comparées par `Id`** : si on modifie le `Title` d'un film existant (même `Id`), EF Core générera un `UPDATE` dans la prochaine migration.
- **Le Seed Data est géré par les migrations** : après avoir ajouté `HasData()`, il faut créer une nouvelle migration et l'appliquer.

---

## 9. Les opérations CRUD

### Théorie — Qu'est-ce que le CRUD ?

Le **CRUD** désigne les quatre opérations fondamentales pour manipuler des données :

| Lettre | Opération | SQL équivalent | Méthode EF Core |
|--------|-----------|---------------|-----------------|
| **C** | Create | `INSERT` | `context.Films.Add(film)` |
| **R** | Read | `SELECT` | `context.Films.Where(...)`, `context.Films.FirstOrDefault(...)` |
| **U** | Update | `UPDATE` | Modifier les propriétés d'un objet suivi |
| **D** | Delete | `DELETE` | `context.Films.Remove(film)` |

### Théorie — Le Change Tracking

EF Core utilise un mécanisme de **Change Tracking** (suivi des modifications). Quand on récupère une entité depuis la base de données, EF Core la « suit ». Toute modification de ses propriétés est détectée automatiquement. L'appel à `SaveChanges()` génère alors les requêtes SQL correspondantes (`INSERT`, `UPDATE`, `DELETE`) et les exécute en base.

### Create — Ajouter un film

```csharp
// 1. Créer l'objet C#
Film film = new Film
{
    Title = title,
    ReleasedYear = releasedYear
};

// 2. Ajouter à la collection (EF Core le marque comme "Added")
context.Films.Add(film);

// 3. Sauvegarder → génère un INSERT en SQL
context.SaveChanges();
```

Le `Id` (GUID) est généré automatiquement par EF Core. Le `CreatedAt` est rempli par SQL Server grâce à `GETDATE()` (configuré via `HasDefaultValueSql`). On n'a pas besoin de fournir ces valeurs.

### Read — Lire les données

**Récupérer tous les films (triés par année décroissante) :**

```csharp
IQueryable<Film> films = context.Films
    .Select(f => f)
    .OrderByDescending(f => f.ReleasedYear);

foreach (Film film in films)
{
    Console.WriteLine($" - {film.Title.PadRight(50)} sorti en {film.ReleasedYear}");
}
```

**Rechercher un film par titre :**

```csharp
var film = context.Films
    .Where(f => f.Title == title)
    .FirstOrDefault();

if (film != null)
{
    Console.WriteLine($"Film trouvé: {film.Title} {film.CreatedAt}");
}
else
{
    Console.WriteLine($"Aucun film trouvé sous le nom de '{title}'");
}
```

### Théorie — `IQueryable` vs `IEnumerable`

| Type | Exécution | Utilisation |
|------|-----------|-------------|
| **`IQueryable<T>`** | **Différée** — la requête SQL n'est exécutée que lorsqu'on itère (dans le `foreach`, ou lors d'un appel à `ToList()`, `FirstOrDefault()`, etc.) | Requêtes vers la base de données |
| **`IEnumerable<T>`** | **En mémoire** — tout est déjà chargé en mémoire | Collections C# classiques |

L'avantage d'`IQueryable` est qu'EF Core peut **composer** la requête : les filtres (`Where`), tris (`OrderBy`), projections (`Select`) sont traduits en SQL et exécutés côté serveur, ce qui est bien plus performant que de tout charger en mémoire.

### Théorie — Les méthodes LINQ courantes avec EF Core

| Méthode LINQ | SQL généré | Rôle |
|-------------|-----------|------|
| `Where(f => ...)` | `WHERE ...` | Filtrer les résultats |
| `OrderBy(f => ...)` / `OrderByDescending` | `ORDER BY ...` | Trier les résultats |
| `Select(f => ...)` | `SELECT ...` | Projeter (choisir les colonnes) |
| `FirstOrDefault(f => ...)` | `SELECT TOP 1 ...` | Récupérer le premier résultat ou `null` |
| `ToList()` | Exécute la requête | Matérialiser en `List<T>` |
| `Count()` | `SELECT COUNT(*)` | Compter les résultats |

### Update et Delete (à venir)

Les opérations Update et Delete suivent le même principe : on récupère l'entité, on la modifie ou on appelle `Remove()`, puis on appelle `SaveChanges()`.

```csharp
// UPDATE — Modifier un film existant
var film = context.Films.FirstOrDefault(f => f.Title == "Inception");
if (film != null)
{
    film.Title = "Inception (2010)";   // EF Core détecte la modification
    context.SaveChanges();              // → génère un UPDATE
}

// DELETE — Supprimer un film
var film = context.Films.FirstOrDefault(f => f.Title == "Inception");
if (film != null)
{
    context.Films.Remove(film);         // EF Core le marque comme "Deleted"
    context.SaveChanges();              // → génère un DELETE
}
```

---

## 10. Les Relations entre entités

### Théorie — Les types de relations

Dans une base de données relationnelle, les tables sont rarement isolées. Elles sont liées entre elles par des **relations** (ou associations). EF Core supporte les trois types de relations classiques :

| Relation | Principe | Exemple concret |
|----------|----------|-----------------|
| **One-to-One** (1:1) | Une entité A est liée à **exactement une** entité B, et inversement. | Un `User` possède un unique `UserProfile` |
| **One-to-Many** (1:N) | Une entité A est liée à **plusieurs** entités B, mais chaque B n'appartient qu'à **un seul** A. | Un `Director` réalise **plusieurs** `Film`, mais chaque `Film` n'a qu'**un seul** `Director` |
| **Many-to-Many** (N:N) | Plusieurs entités A sont liées à plusieurs entités B. | Un `Film` a **plusieurs** `Actor`, et un `Actor` joue dans **plusieurs** `Film` |

### Théorie — Navigation Properties et Foreign Keys

Pour matérialiser une relation en EF Core, on utilise deux concepts :

- **Navigation Property** : Une propriété C# qui permet de « naviguer » d'une entité vers une autre. Elle peut être une référence (un seul objet) ou une collection (`ICollection<T>`, `List<T>`).
- **Foreign Key** (clé étrangère) : La colonne en base de données qui stocke la référence vers l'autre table. En C#, c'est une propriété de type `Guid`, `int`, etc.

```
Côté C# (Navigation)          Côté SQL (Foreign Key)
─────────────────────          ──────────────────────
film.Director                  Films.DirectorId → Directors.Id
director.Films                 (pas de colonne, c'est l'inverse)
```

---

### 10.1. Relation One-to-Many (1:N)

C'est la relation **la plus courante**. Exemple : un réalisateur peut avoir réalisé plusieurs films, mais chaque film n'a qu'un seul réalisateur.

#### Étape 1 — Modifier les entités

**`Entities/Director.cs`** — Ajouter la collection de films :

```csharp
namespace DemoEFCore.Entities;

public class Director
{
    public Guid Id { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;

    // Navigation Property — Un réalisateur a PLUSIEURS films
    public ICollection<Film> Films { get; set; } = new List<Film>();
}
```

**`Entities/Film.cs`** — Ajouter la foreign key et la navigation vers le réalisateur :

```csharp
namespace DemoEFCore.Entities;

public class Film
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public DateTime? CreatedAt { get; set; }

    // Foreign Key — Référence vers le réalisateur
    public Guid? DirectorId { get; set; }

    // Navigation Property — Chaque film a UN réalisateur
    public Director? Director { get; set; }
}
```

> **`Guid?`** (nullable) : La FK est optionnelle ici — un film peut ne pas encore avoir de réalisateur assigné. Si on veut rendre la relation **obligatoire**, on utilise `Guid` sans le `?`.

#### Étape 2 — Configurer avec Fluent API

**`Configurations/FilmConfiguration.cs`** — Ajouter la configuration de la relation :

```csharp
// Dans la méthode Configure()

// Relation One-to-Many : Director (1) → Films (N)
builder.HasOne(f => f.Director)           // Chaque Film a UN Director
    .WithMany(d => d.Films)               // Ce Director a PLUSIEURS Films
    .HasForeignKey(f => f.DirectorId)     // La FK est Film.DirectorId
    .OnDelete(DeleteBehavior.SetNull);    // Si on supprime le Director → FK mise à null
```

#### Théorie — Les comportements de suppression (`OnDelete`)

| Comportement | Effet quand le parent est supprimé |
|-------------|-----------------------------------|
| `Cascade` | Les enfants sont **supprimés** automatiquement |
| `SetNull` | La FK des enfants est mise à **`NULL`** (nécessite une FK nullable) |
| `Restrict` | La suppression est **bloquée** si des enfants existent |
| `NoAction` | Aucune action côté EF Core (délégué à la base de données) |

#### Étape 3 — Créer et appliquer la migration

```bash
dotnet ef migrations add AddDirectorToFilm
dotnet ef database update
```

EF Core génère automatiquement une colonne `DirectorId` dans la table `Films` avec une contrainte `FOREIGN KEY` vers `Directors.Id`.

---

### 10.2. Relation One-to-One (1:1)

Exemple : chaque utilisateur possède un unique profil, et chaque profil appartient à un unique utilisateur.

#### Étape 1 — Créer l'entité dépendante

**`Entities/UserProfile.cs`** :

```csharp
namespace DemoEFCore.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }

    // Foreign Key (aussi clé primaire dans certains cas)
    public Guid UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}
```

**`Entities/User.cs`** — Ajouter la navigation inverse :

```csharp
namespace DemoEFCore.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Lastname { get; set; }
    public string? Firstname { get; set; }

    // Navigation Property — Un utilisateur a UN profil
    public UserProfile? Profile { get; set; }
}
```

#### Étape 2 — Configurer avec Fluent API

**`Configurations/UserProfileConfiguration.cs`** :

```csharp
public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(up => up.Id);

        // Relation One-to-One : User (1) ↔ UserProfile (1)
        builder.HasOne(up => up.User)         // Chaque UserProfile a UN User
            .WithOne(u => u.Profile)          // Ce User a UN UserProfile
            .HasForeignKey<UserProfile>(up => up.UserId); // La FK est dans UserProfile
    }
}
```

#### Théorie — Qui porte la Foreign Key ?

Dans une relation 1:1, on doit **choisir** quel côté porte la FK. On l'indique avec `HasForeignKey<T>()` où `T` est l'entité qui contient la FK. En général, c'est l'entité **dépendante** (celle qui ne peut pas exister sans l'autre).

---

### 10.3. Relation Many-to-Many (N:N)

Exemple : un film peut avoir plusieurs acteurs, et un acteur peut jouer dans plusieurs films.

#### Théorie — La table de jointure

Une relation N:N ne peut pas être représentée directement en SQL. Il faut une **table de jointure** (aussi appelée table pivot ou table d'association) qui contient les deux FK.

```
Films ←──── FilmActors ────→ Actors
 Id          FilmId            Id
             ActorId
```

EF Core offre **deux approches** pour gérer le Many-to-Many :

| Approche | Quand l'utiliser |
|----------|-----------------|
| **Implicite** (EF Core 5+) | La table de jointure est invisible, EF Core la gère tout seul. Suffisant si on n'a pas besoin de données supplémentaires sur la relation. |
| **Explicite** | On crée manuellement l'entité de jointure. Nécessaire si on veut ajouter des colonnes à la relation (ex: `Role`, `DateCasting`). |

#### Approche implicite (simple)

**`Entities/Actor.cs`** :

```csharp
namespace DemoEFCore.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string Lastname { get; set; } = null!;
    public string Firstname { get; set; } = null!;

    // Navigation Property — Un acteur joue dans PLUSIEURS films
    public ICollection<Film> Films { get; set; } = new List<Film>();
}
```

**`Entities/Film.cs`** — Ajouter la navigation :

```csharp
// Ajouter dans la classe Film :
public ICollection<Actor> Actors { get; set; } = new List<Actor>();
```

**Configuration Fluent API** :

```csharp
// Dans FilmConfiguration ou ActorConfiguration
builder.HasMany(f => f.Actors)
    .WithMany(a => a.Films);
// EF Core crée automatiquement la table de jointure "ActorFilm"
```

#### Approche explicite (avec entité de jointure)

Utile quand on veut stocker des informations supplémentaires sur la relation, par exemple le rôle joué par l'acteur dans le film.

**`Entities/FilmActor.cs`** — L'entité de jointure :

```csharp
namespace DemoEFCore.Entities;

public class FilmActor
{
    public Guid FilmId { get; set; }
    public Film Film { get; set; } = null!;

    public Guid ActorId { get; set; }
    public Actor Actor { get; set; } = null!;

    // Donnée supplémentaire sur la relation
    public string? Role { get; set; }
}
```

**Configuration Fluent API** :

```csharp
public class FilmActorConfiguration : IEntityTypeConfiguration<FilmActor>
{
    public void Configure(EntityTypeBuilder<FilmActor> builder)
    {
        builder.ToTable("FilmActors");

        // Clé primaire composite (les deux FK ensemble)
        builder.HasKey(fa => new { fa.FilmId, fa.ActorId });

        // Relation Film (1) → FilmActor (N)
        builder.HasOne(fa => fa.Film)
            .WithMany(f => f.FilmActors)
            .HasForeignKey(fa => fa.FilmId);

        // Relation Actor (1) → FilmActor (N)
        builder.HasOne(fa => fa.Actor)
            .WithMany(a => a.FilmActors)
            .HasForeignKey(fa => fa.ActorId);
    }
}
```

> Avec l'approche explicite, les navigation properties dans `Film` et `Actor` deviennent `ICollection<FilmActor>` au lieu de `ICollection<Film>` / `ICollection<Actor>`.

### Récapitulatif des relations

| Relation | Navigation côté A | Navigation côté B | Configuration Fluent API |
|----------|------------------|------------------|--------------------------|
| **1:1** | `B? Prop` | `A Prop` | `HasOne().WithOne().HasForeignKey<B>()` |
| **1:N** | `ICollection<B>` | `A? Prop` + `Guid? AId` | `HasOne().WithMany().HasForeignKey()` |
| **N:N (implicite)** | `ICollection<B>` | `ICollection<A>` | `HasMany().WithMany()` |
| **N:N (explicite)** | `ICollection<AB>` | `ICollection<AB>` | Deux `HasOne().WithMany()` sur l'entité de jointure |

### Théorie — Le Lazy Loading, Eager Loading et Explicit Loading

Quand on accède à une navigation property, EF Core ne charge **pas** automatiquement les données liées. Il existe trois stratégies :

| Stratégie | Comment | Quand l'utiliser |
|-----------|---------|-----------------|
| **Eager Loading** | `.Include(f => f.Director)` | On sait à l'avance qu'on aura besoin des données liées |
| **Explicit Loading** | `context.Entry(film).Reference(f => f.Director).Load()` | On décide au cas par cas, après coup |
| **Lazy Loading** | Installer un package supplémentaire + propriétés `virtual` | Automatique mais peut causer des problèmes de performance (N+1 queries) |

**Exemple avec Eager Loading (recommandé)** :

```csharp
// Charger les films AVEC leur réalisateur en une seule requête
var films = context.Films
    .Include(f => f.Director)    // JOIN en SQL
    .OrderByDescending(f => f.ReleasedYear)
    .ToList();

foreach (var film in films)
{
    string directorName = film.Director != null
        ? $"{film.Director.Firstname} {film.Director.Lastname}"
        : "Inconnu";
    Console.WriteLine($"{film.Title} — réalisé par {directorName}");
}
```

Sans le `.Include()`, `film.Director` serait toujours `null` même si un `DirectorId` existe en base.

---

## 11. Le point d'entrée et le menu console

### Le `Program.cs`

Le point d'entrée de l'application crée une instance du `DataContext` et lance le menu interactif :

```csharp
using DemoEFCore;

using DataContext context = new DataContext();

Menu.AfficherMenu(context);
```

Le mot-clé `using` devant la déclaration assure que le `DataContext` sera **disposé** (libéré) automatiquement à la fin du programme. Le `DbContext` implémente `IDisposable` car il gère une connexion à la base de données qu'il faut fermer proprement.

### La classe `Menu`

La classe `Menu` est une classe `static` qui organise l'interface console en sous-menus imbriqués :

```
Menu principal
├── 1. Démonstrations sur le CRUD
│   ├── 1. Create — Ajouter un film
│   ├── 2. Read — Récupérer des données
│   │   ├── 1. Afficher tous les films
│   │   └── 2. Rechercher un film par titre
│   ├── 3. Update — (à implémenter)
│   └── 4. Delete — (à implémenter)
└── 0. Quitter
```

### Théorie — Le pattern de boucle interactive

Le menu utilise un pattern classique pour les applications console : une boucle `while` avec un booléen de contrôle.

```csharp
bool continuer = true;
while (continuer)
{
    // 1. Afficher les options
    // 2. Lire le choix de l'utilisateur
    // 3. Exécuter l'action correspondante (switch)
    // 4. Le choix "0" met continuer = false pour sortir
}
```

Ce pattern est répété à chaque niveau de menu, ce qui permet de revenir au menu parent en sortant simplement de la boucle.

---

## 12. Récapitulatif de l'architecture

```
DemoEFCore/
├── Entities/                    # Les classes C# = tables SQL
│   ├── Director.cs
│   ├── Film.cs
│   └── User.cs
├── Configurations/              # Mapping Fluent API (entité ↔ table)
│   ├── DirectorConfiguration.cs
│   ├── FilmConfiguration.cs
│   └── UserConfiguration.cs
├── Migrations/                  # Historique versionné du schéma BDD
│   ├── 20260225..._InitialMigration.cs
│   ├── 20260226..._AddConfigurations.cs
│   └── DataContextModelSnapshot.cs
├── DataContext.cs               # Le DbContext (point d'entrée EF Core)
├── Menu.cs                      # Interface console interactive (CRUD)
├── Program.cs                   # Point d'entrée de l'application
└── DemoEFCore.csproj            # Configuration du projet .NET
```

### Flux de travail typique (Code First)

```
1. Créer / modifier une entité C#
        ↓
2. Créer / modifier la configuration Fluent API
        ↓
3. Générer une migration :  dotnet ef migrations add <Nom>
        ↓
4. Appliquer à la BDD :     dotnet ef database update
        ↓
5. Utiliser le DbContext pour les opérations CRUD
        ↓
6. Répéter à chaque évolution du modèle
```

---

## Licence

Ce projet est distribué sous licence MIT. Voir le fichier `LICENSE.txt`.
