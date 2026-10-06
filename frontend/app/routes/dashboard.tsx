import { SidebarProvider, useSidebar } from "~/context/SidebarContext";
import { cn } from "~/utils";
import { Outlet, redirect } from "react-router";
import Header from "~/components/layout/Header";
import Sidebar from "~/components/layout/Sidebar";
import Backdrop from "~/components/layout/Backdrop";
import { getCurrentUser } from "~/services/localStorage/users";
import { useState } from "react";
import AIChatbot from "~/components/layout/AIChatbot";

export async function clientLoader() {
  const currentUserId = getCurrentUser();

  if (!currentUserId) {
    throw redirect("/");
  }

  return null;
}

const LayoutContent: React.FC = () => {
  const { isExpanded, isHovered, isMobileOpen } = useSidebar();
  const [botOpen, setBotOpen] = useState<boolean>(false);

  return (
    <div className="min-h-screen xl:flex">
      <Sidebar onButtonClick={() => setBotOpen(true)} />
      <Backdrop />
      <div
        className={cn(
          "flex-1 transition-[margin] duration-300 ease-in-out",
          isExpanded || isHovered ? "xl:ms-72.5" : "xl:ms-22.5",
          isMobileOpen ? "ms-0" : "",
        )}
      >
        <Header />
        <main className="mx-auto max-w-(--breakpoint-2xl) p-4 md:p-6">
          {botOpen && <AIChatbot onClose={() => setBotOpen(false)} />}
          <Outlet />
        </main>
      </div>
    </div>
  );
};

const AppLayout: React.FC = () => {
  return (
    <SidebarProvider>
      <LayoutContent />
    </SidebarProvider>
  );
};

export default AppLayout;
