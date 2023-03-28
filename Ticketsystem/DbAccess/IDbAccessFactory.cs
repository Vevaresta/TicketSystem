namespace Ticketsystem.DbAccess
{
    public interface IDbAccessFactory
    {
        public RolePermissionsDbAccess GetRolePermissionsDbAccess();
        public RolesDbAccess GetRolesDbAccess();
        public RolesToDisplayDbAccess GetRolesToDisplayDbAccess();
        public TicketTypesDbAccess GetTicketTypesDbAccess();
        public TicketStatusesDbAccess GetTicketStatusesDbAccess();
        public TicketChangesDbAccess GetTicketChangesDbAccess();
        public ITicketsClientsDbAccess GetTicketsClientsDbAccess<T>();
    }
}
