namespace OnlyFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PostLikePosts", new[] { "PostLike_PostId", "PostLike_UserId" }, "dbo.PostLikes");
            DropForeignKey("dbo.PostLikePosts", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.ApplicationUserPostLikes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserPostLikes", new[] { "PostLike_PostId", "PostLike_UserId" }, "dbo.PostLikes");
            DropIndex("dbo.PostLikePosts", new[] { "PostLike_PostId", "PostLike_UserId" });
            DropIndex("dbo.PostLikePosts", new[] { "Post_PostId" });
            DropIndex("dbo.ApplicationUserPostLikes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserPostLikes", new[] { "PostLike_PostId", "PostLike_UserId" });
            AddColumn("dbo.PostLikes", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.PostLikes", "PostId");
            CreateIndex("dbo.PostLikes", "ApplicationUser_Id");
            AddForeignKey("dbo.PostLikes", "PostId", "dbo.Posts", "PostId", cascadeDelete: true);
            AddForeignKey("dbo.PostLikes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.PostLikePosts");
            DropTable("dbo.ApplicationUserPostLikes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserPostLikes",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        PostLike_PostId = c.Int(nullable: false),
                        PostLike_UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.PostLike_PostId, t.PostLike_UserId });
            
            CreateTable(
                "dbo.PostLikePosts",
                c => new
                    {
                        PostLike_PostId = c.Int(nullable: false),
                        PostLike_UserId = c.String(nullable: false, maxLength: 128),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostLike_PostId, t.PostLike_UserId, t.Post_PostId });
            
            DropForeignKey("dbo.PostLikes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostLikes", "PostId", "dbo.Posts");
            DropIndex("dbo.PostLikes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PostLikes", new[] { "PostId" });
            DropColumn("dbo.PostLikes", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserPostLikes", new[] { "PostLike_PostId", "PostLike_UserId" });
            CreateIndex("dbo.ApplicationUserPostLikes", "ApplicationUser_Id");
            CreateIndex("dbo.PostLikePosts", "Post_PostId");
            CreateIndex("dbo.PostLikePosts", new[] { "PostLike_PostId", "PostLike_UserId" });
            AddForeignKey("dbo.ApplicationUserPostLikes", new[] { "PostLike_PostId", "PostLike_UserId" }, "dbo.PostLikes", new[] { "PostId", "UserId" }, cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserPostLikes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostLikePosts", "Post_PostId", "dbo.Posts", "PostId", cascadeDelete: true);
            AddForeignKey("dbo.PostLikePosts", new[] { "PostLike_PostId", "PostLike_UserId" }, "dbo.PostLikes", new[] { "PostId", "UserId" }, cascadeDelete: true);
        }
    }
}
