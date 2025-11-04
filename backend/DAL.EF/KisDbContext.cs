using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF;

public class KisDbContext(DbContextOptions<KisDbContext> options) : DbContext(options) {

}
