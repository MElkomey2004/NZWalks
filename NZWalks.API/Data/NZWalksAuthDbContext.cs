using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
	public class NZWalksAuthDbContext : IdentityDbContext
	{
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options): base(options)
        {
            
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var readerRoleId = "d8739985-f978-42d5-a629-b6cb7db5d202";
			var writerRoleId = "9ed6845e-ebcb-4abe-99ab-4e7d9185968f";

			var roles = new List<IdentityRole>
			{

				new IdentityRole
				{
					Id = readerRoleId,
					ConcurrencyStamp = readerRoleId,
					Name = "Reader",
					NormalizedName = "Reader".ToUpper()
				},
					new IdentityRole
				{
					Id = writerRoleId,
					ConcurrencyStamp = writerRoleId,
					Name = "Writer",
					NormalizedName = "Writer".ToUpper()
				}
			};

			builder.Entity<IdentityRole>().HasData(roles);
		}

	}
}
