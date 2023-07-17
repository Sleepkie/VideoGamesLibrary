﻿using System;
using System.Collections.Generic;

namespace VideoGamesLibrary.Models;

public partial class Developer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
