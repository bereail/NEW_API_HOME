﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace toner_API.Models
{
    public partial class Carga
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdToner { get; set; }
        public int? IdService { get; set; }
        public DateTime? CargaAt { get; set; }
        public int? Cant { get; set; }

        public virtual Toner IdTonerNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}