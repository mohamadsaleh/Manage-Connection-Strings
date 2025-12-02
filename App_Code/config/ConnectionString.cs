using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Represents a database connection string with type information
/// </summary>
public class ConnectionString
{
    public int ConnectionStringId { get; set; }
    public int UserId { get; set; }
    public string ConnectionStringName { get; set; }
    public string ConnectionStringValue { get; set; }
    public string Type { get; set; }
}