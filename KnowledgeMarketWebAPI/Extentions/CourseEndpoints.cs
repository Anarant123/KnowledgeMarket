using KnowledgeMarketWebAPI.Auth;
using KnowledgeMarketWebAPI.Contracts.Requests.Models;
using KnowledgeMarketWebAPI.Contracts.Responses;
using KnowledgeMarketWebAPI.Data;
using KnowledgeMarketWebAPI.Data.Models;
using KnowledgeMarketWebAPI.Data.Models.db;
using KnowledgeMarketWebAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KnowledgeMarketWebAPI.Extentions;

public static class CourseEndpoints
{
    public static IEndpointRouteBuilder MapCourseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Courses/").WithTags("Courses");

        group.MapGet("GetCourse", async (int id, KnowledgeMarketContext context, ClaimsPrincipal user) =>
        {
            var ad = await context.Courses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ad is null) return Results.NotFound();

            var dto = CourseDTO.Map(ad, false);
            var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (userIdClaim is not null)
            {
                var userId = int.Parse(userIdClaim);
                dto.IsPurchated = await context.PurchasedСourses.AnyAsync(x => x.UserId == userId && x.CourseId == id);
            }

            return Results.Ok(dto);
        }).AllowAnonymous();

        group.MapGet("GetСources", async (KnowledgeMarketContext context) =>
        {
            var courses = await context.Courses
                .Include(x => x.User)
                .ToListAsync();
            return Results.Ok(courses);
        }).AllowAnonymous();

        group.MapGet("GetPurchatedCourses", async (KnowledgeMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var courses = await context.PurchasedСourses
                .Include(x => x.Course)
                .Where(x => x.UserId == userId)
                .Select(x => x.Course)
                .ToListAsync();


            return Results.Ok(courses);
        });

        group.MapPost("Addition", async (AddCourseModel model, KnowledgeMarketContext context, ClaimsPrincipal user,
            FileManager fileManager) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var course = new Course
            {
                Price = model.Price,
                Name = model.Name,
                Description = model.Description,
                Photo = await fileManager.SaveCoursePhoto(null)
            };

            context.Courses.Add(course);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.Conflict();
            }

            return Results.Ok(course);
        }).RequireAuthorization(Policies.Admin);

        group.MapPut("{id:int}/Photo", async (int id, IFormFile? photo, KnowledgeMarketContext context,
            FileManager fileManager, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var course = await context.Courses
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course is null) return Results.NotFound();

            if (course.UserId != userId) return Results.Forbid();

            course.Photo = await fileManager.SaveCoursePhoto(photo);

            await context.SaveChangesAsync();

            return Results.Ok(course);
        }).RequireAuthorization(Policies.Admin);

        group.MapPut("Update", async (UpdateCourseModel model, KnowledgeMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var course = await context.Courses
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (course is null) return Results.NotFound();

            if (course.UserId != userId) return Results.Forbid();

            if (model.Price is not null) course.Price = model.Price.Value;
            if (model.Name is not null) course.Name = model.Name;
            if (model.Description is not null) course.Description = model.Description;
            if (model.Author is not null) course.Author = model.Author;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.Conflict();
            }

            return Results.Ok(course);
        }).RequireAuthorization(Policies.Admin);

        group.MapDelete("Delete", async (int id, KnowledgeMarketContext context, ClaimsPrincipal user) =>
        {
            var userId = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            var res = Enum.TryParse(user.Claims.First(x => x.Type == "rightId").Value, out RoleType userRightId);
            if (!res) return Results.BadRequest();

            var ad = await context.Courses.FindAsync(id);
            if (ad is null) return Results.NotFound();

            if (userRightId != RoleType.Admin && userId != ad.UserId) return Results.Forbid();

            ad.IsDeleted = true;
            await context.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(Policies.Admin);

        return app;
    }
}
