"use client";
import { queryClient } from "@/shared/api/query-client";
import { SidebarProvider, SidebarInset } from "@/shared/components/ui/sidebar";
import { QueryClientProvider } from "@tanstack/react-query";
import Header from "../header/header";
import { AppSidebar } from "../sidebar/app-sidebar";

export function Layout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <QueryClientProvider client={queryClient}>
      <SidebarProvider>
        <div className="flex h-screen w-full">
          <AppSidebar />
          <div className="flex-1 flex-col min-w-0">
            <SidebarInset>
              <Header />
              <main className="flex-1 overflow-auto p-10">{children}</main>
            </SidebarInset>
          </div>
        </div>
      </SidebarProvider>
    </QueryClientProvider>
  );
}
