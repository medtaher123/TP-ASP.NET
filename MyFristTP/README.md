## Systèmes de Routage dans ASP.NET Core

ASP.NET Core propose plusieurs méthodes de routage pour diriger les requêtes HTTP vers les contrôleurs appropriés. Les principaux types de routage sont le **routage conventionnel** , le **routage par attribut** et le **routage mixte** .

### 1. Routage Conventionnel

Le routage conventionnel repose sur des conventions prédéfinies pour créer des routes à partir des noms de contrôleurs et des actions. Ce type de routage est généralement défini dans la classe `Startup.cs`, où l'on configure les modèles de routage.**Exemple de configuration :**

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
}
```

Dans ce cas, les routes sont généralement formées comme : `/NomDuContrôleur/NomDeLAction/Id`. Par exemple, une requête à `/Movie/Details/1` invoquerait l'action `Details` dans le contrôleur `Movie` avec `1` comme paramètre.

### 2. Routage par Attribut

Le routage par attribut utilise des annotations directement au-dessus des méthodes d'action pour définir les routes. Cela permet de lier explicitement chaque action à son URL.
**Exemple :**

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

Les routes sont directement liées aux méthodes, et peuvent inclure des paramètres de manière intuitive.

### 3. Routage Mixte

Le routage mixte combine les deux approches précédentes, permettant d'utiliser à la fois le routage conventionnel et le routage par attribut dans une même application.
**Exemple de configuration :**

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");

        // Ajout de routes par attribut
        routes.MapMvcAttributeRoutes();
    });
}
```

Cela permet aux développeurs de choisir la méthode de routage la plus appropriée pour chaque situation. Les routes complexes peuvent être gérées via le routage par attribut, tandis que les routes simples peuvent suivre la convention standard.
