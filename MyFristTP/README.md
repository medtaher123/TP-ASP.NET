## Systèmes de Routage dans ASP.NET Core (Version la plus récente)

ASP.NET Core offre plusieurs approches pour gérer le routage des requêtes HTTP vers les contrôleurs appropriés. Les trois principaux types de routage sont : **routage conventionnel** , **routage par attribut** et **routage mixte** .

### 1. Routage Conventionnel

Le **routage conventionnel** s'appuie sur des conventions pour définir les routes en fonction des noms de contrôleurs et d'actions. Dans la version la plus récente d'ASP.NET Core, le routage est défini via `MapControllerRoute` dans le fichier `Program.cs`.
Voici un exemple de configuration pour un routage conventionnel dans ASP.NET Core 6.0+ :

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

```csharp
public class CustomerController : Controller
    {

        public IActionResult Index()
        {
            // logique pour recuperer les customers
        }


        public IActionResult Details(int id)
        {
            // logique pour recuperer uncustomer par son id
        }


    }
```

- **Exemple de contrôleur conventionnel : [CustomerController](Controllers/CustomerController.cs)**

Dans ce cas, une requête à `/Customer/CustomerDetails/1` serait dirigée vers l'action `CustomerDetails` du contrôleur `Customer`, avec `1` comme paramètre `id`. Les routes suivent le modèle : `/NomDuContrôleur/NomDeLAction/Id`.

### 2. Routage par Attribut

Le **routage par attribut** permet d'associer directement les actions aux routes à l'aide d'annotations au-dessus des méthodes d'action. Cela offre plus de flexibilité et de contrôle sur les URLs, permettant de définir des routes personnalisées pour chaque action.**Exemple :**

```csharp
[Route("movies")]
public class MovieController : Controller
{
    [HttpGet("{id}")]
    public IActionResult GetMovie(int id)
    {
        // Logique pour récupérer un film par son ID
    }

    [HttpPost]
    public IActionResult AddMovie([FromBody] Movie movie)
    {
        // Logique pour ajouter un nouveau film
    }
}
```

- **Exemple de routage par attribut : [MovieController](Controllers/MovieController.cs)**

Dans cet exemple, la route `/movies/1` accéderait directement à l’action `GetMovie` avec l’ID passé comme paramètre.

### 3. Routage Mixte

Le **routage mixte** combine le routage conventionnel et le routage par attribut, permettant aux développeurs d'utiliser l'une ou l'autre des approches dans la même application selon les besoins.
Voici comment le configurer dans une application ASP.NET Core 6.0+ :

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Les routes avec des attributs sont automatiquement prises en charge
app.Run();
```

Dans ce modèle, vous pouvez définir des routes conventionnelles tout en ajoutant des routes personnalisées via des attributs au niveau des contrôleurs ou des actions. Cela permet d’utiliser des routes simples avec la convention standard, tout en profitant du routage par attribut pour des cas plus spécifiques ou complexes.

### Conclusion

Avec ASP.NET Core 6.0 et les versions suivantes, le **routage conventionnel** est défini à l’aide de `MapControllerRoute`, offrant une approche simple et efficace pour gérer les routes dans une application. Pour les scénarios où vous avez besoin d'un contrôle plus précis sur les routes, vous pouvez utiliser le **routage par attribut** ou un mélange des deux avec le **routage mixte** .Le fichier `Program.cs` dans ASP.NET Core 6.0 est maintenant beaucoup plus simple, consolidant les configurations du pipeline dans un seul endroit, rendant la gestion des routes plus claire et intuitive.
