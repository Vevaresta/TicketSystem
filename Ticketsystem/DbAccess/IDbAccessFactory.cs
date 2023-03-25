namespace Ticketsystem.DbAccess
{
    public interface IDbAccessFactory
    {
        public RolePermissionsDbAccess RolePermissionsDbAccess { get; }
        public RolesDbAccess RolesDbAccess { get; }
        public RolesToDisplayDbAccess RolesToDisplayDbAccess { get; }
        public TicketsDbAccess TicketsDbAccess { get; }
        public TicketTypesDbAccess TicketTypesDbAccess { get; }
        public TicketStatusesDbAccess TicketStatusesDbAccess { get; }
        public TicketChangesDbAccess TicketChangesDbAccess { get; }
        public ClientsDbAccess ClientsDbAccess { get; }
    }
}
