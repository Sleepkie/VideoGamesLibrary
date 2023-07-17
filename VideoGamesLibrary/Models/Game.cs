using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VideoGamesLibrary.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Developerid { get; set; }

    public virtual Developer? Developer { get; set; }

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
