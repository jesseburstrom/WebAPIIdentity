using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIIdentity.Models
{
    public class Highscore
    {
        public Highscore(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }


}
