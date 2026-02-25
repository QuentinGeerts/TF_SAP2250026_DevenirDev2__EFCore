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
9. [Récapitulatif de l'architecture](#9-récapitulatif-de-larchitecture)

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
    public Guid Id { get; set; }          // Clé primaire
    public string Title { get; set; } = null!;
    public int ReleasedYear { get; set; }
    public DateTime CreatedAt { get; set; }
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
using DemoEFCore.Configurations;
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
dotnet ef migrations add NomDeLaMigration
Add-Migration NomDeLaMigration

# Appliquer les migrations en attente à la base de données
dotnet ef database update
Update-Database

# Annuler la dernière migration (si elle n'a pas été appliquée)
dotnet ef migrations remove
Remove-Migration

# Revenir à une migration spécifique
dotnet ef database update NomDeLaMigration
Update-Database NomDeLaMigration
```

### Migrations du projet

**Migration 1 — `InitialMigration`** : Crée les trois tables `Directors`, `Films` et `Users` avec leurs colonnes de base.

```
dotnet ef migrations add InitialMigration
dotnet ef database update
```

**Migration 2 — `FilmConfiguration`** : Ajoute la colonne `CreatedAt` à la table `Films` (car elle a été ajoutée à l'entité `Film` et configurée avec `HasDefaultValueSql`).

```
dotnet ef migrations add FilmConfiguration
dotnet ef database update
```

### Fichiers générés par une migration

Chaque migration produit **trois fichiers** (ou met à jour le snapshot) :

| Fichier | Rôle |
|---------|------|
| `YYYYMMDD_NomMigration.cs` | Le code `Up()` / `Down()` qui décrit les changements |
| `YYYYMMDD_NomMigration.Designer.cs` | Snapshot du modèle complet **au moment de cette migration** |
| `DataContextModelSnapshot.cs` | Snapshot du modèle **actuel** (mis à jour à chaque migration) |

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

## 9. Récapitulatif de l'architecture

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
│   ├── 20260225..._FilmConfiguration.cs
│   └── DataContextModelSnapshot.cs
├── DataContext.cs               # Le DbContext (point d'entrée EF Core)
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
5. Répéter à chaque évolution du modèle
```

---

## Licence

Ce projet est distribué sous licence MIT. Voir le fichier `LICENSE.txt`.
