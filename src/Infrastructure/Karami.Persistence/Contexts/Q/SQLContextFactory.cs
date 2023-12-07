using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/*کلاس زیر برای شناسایی Context ما توسط دستورات کامندی EF Core می باشد*/
namespace Karami.Persistence.Contexts.Q;

/*در این قسمت برای آن که EFCore بتواند Context ما را شناسایی کند تا فایل های Migration مربوطه را بسازد ، باید از دستورات زیر استفاده کنیم*/
/*این کار برای آن است که ما می خواهیم فایل های Migration خود را در لایه ای جدا از لایه اصلی که فایل StartUp یا Program در آن است ، ایجاد نماییم*/
public class SQLContextFactory : IDesignTimeDbContextFactory<SQLContext>
{
    public SQLContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<SQLContext> builder = new DbContextOptionsBuilder<SQLContext>();
        
        builder.UseSqlServer("Something!");
        
        return new SQLContext(builder.Options);
    }
}