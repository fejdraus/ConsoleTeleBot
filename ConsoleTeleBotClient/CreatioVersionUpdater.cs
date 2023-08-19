using Hangfire.Dashboard;

namespace ConsoleTeleBot;

public class HangfireOpenAuthorizationFilter : IDashboardAuthorizationFilter {
    public bool Authorize(DashboardContext context) {
        return true;
    }
}