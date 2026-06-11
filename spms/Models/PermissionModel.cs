namespace spms.Models
{
    
    public class PermissionModel
    {
        public int Id { get; set; }

        public string PermissionName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

   
    public class RolePermissionModel
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; }
    }

   
    public class RolePermissionViewModel
    {
        public List<RoleModel> Roles { get; set; } = new();
        public List<PermissionModel> Permissions { get; set; } = new();
        public List<RolePermissionModel> RolePermissions { get; set; } = new();
    }
}
