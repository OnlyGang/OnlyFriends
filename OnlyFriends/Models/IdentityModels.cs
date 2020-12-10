using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace OnlyFriends.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [DefaultValue(false)]
        public bool IsPrivate { get; set; }
        public IEnumerable<SelectListItem> AllRoles { get; internal set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupRequest> GroupRequests { get; set; }

        // Asta e pentru Friend Requests (Many-to-Many pe acelasi tabel)
        public virtual ICollection<FriendRequest> SentFriendRequests { get; set; }
        public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; }

        // Asta e pentru User Relations (Many-to-Many pe acelasi tabel)
        public virtual ICollection<UserRelation> MyUserRelations { get; set; }
        public virtual ICollection<UserRelation> OthersUserRelations { get; set; }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) :
        base(store)
        {
        }
        public static ApplicationRoleManager
        Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new
            RoleStore<IdentityRole>(context.Get<ApplicationDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, OnlyFriends.Migrations.Configuration>("DefaultConnection"));

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Group> Groups { get;  set; }
        public DbSet<GroupMember> GroupMembers { get;  set; }
        public DbSet<GroupRequest> GroupRequests { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }

        // Asta e pentru friend requests ca sa functioneze (sterge on delete cascade)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired(pt => pt.Reciever)
                .WithMany(p => p.ReceivedFriendRequests)
                .HasForeignKey(pt => pt.RecieverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired(pt => pt.Sender)
                .WithMany(p => p.SentFriendRequests)
                .HasForeignKey(pt => pt.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRelation>()
                .HasRequired(pt => pt.User1)
                .WithMany(p => p.MyUserRelations)
                .HasForeignKey(pt => pt.User1Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRelation>()
                .HasRequired(pt => pt.User2)
                .WithMany(p => p.OthersUserRelations)
                .HasForeignKey(pt => pt.User2Id)
                .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}