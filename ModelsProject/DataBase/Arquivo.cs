using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;

namespace ModelsProject.DataBase
{
    public class Arquivo : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        public Arquivo(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<Usuario>()
                .HasKey(p => p.ID);
            modelBuilder.Entity<Usuario>()
                .HasAlternateKey(p => p.Email);
            modelBuilder.Entity<Publicacao>()              
                .HasKey(PK => PK.ID);

            modelBuilder.Entity<Comentario>()
               .HasAlternateKey(PK => PK.PublicacaoId);

            modelBuilder.Entity<Comentario>()
               .HasAlternateKey(PK => PK.ComentarioId);

            modelBuilder.Entity<Comentario>()
               .HasAlternateKey(PK => PK.CitacaoId);

            modelBuilder.Entity<Comentario>()
               .HasAlternateKey(PK => PK.AutorComentario);

            modelBuilder.Entity<Comentario>()                
                .HasKey(PK => PK.ID);
        }      
    }
}


