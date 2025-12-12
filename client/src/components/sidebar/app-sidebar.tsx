"use client";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { Home, Plus, ListTodo, type LucideIcon } from "lucide-react";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuItem,
  SidebarMenuButton,
  useSidebar,
} from "../ui/sidebar";
import { routes } from "@/shared/routes";

type MenuItem = {
  href: string;
  label: string;
  icon: LucideIcon;
};

const menuItems: MenuItem[] = [
  {
    href: routes.home,
    label: "Главная",
    icon: Home,
  },
  {
    href: routes.departments,
    label: "Departments",
    icon: Plus,
  },
  {
    href: routes.locations,
    label: "Locations",
    icon: ListTodo,
  },
  {
    href: routes.positions,
    label: "Positions",
    icon: ListTodo,
  },
];

export function AppSidebar() {
  const pathname = usePathname();

  const { setOpenMobile } = useSidebar();

  return (
    <Sidebar collapsible="icon">
      <SidebarHeader>
        <div className="flex items-center gap-2 px-2">
          <div className="flex h-8 w-8 items-center justify-center rounded-full bg-green-600 text-white font-bold text-sm">
            DS
          </div>
          <span className="font-semibold group-data-[collapsible=icon]:hidden">
            DirectoryService
          </span>
        </div>
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu className="space-y-1">
              {menuItems.map((item) => {
                const isActive =
                  pathname === item.href ||
                  pathname.startsWith(item.href + "/");

                return (
                  <SidebarMenuItem key={item.href}>
                    <SidebarMenuButton
                      asChild
                      isActive={isActive}
                      className="hover:bg-accent transition-colors"
                      tooltip={item.label}
                      onClick={() => {
                        setOpenMobile(false);
                      }}
                    >
                      <Link
                        href={item.href}
                        className="flex items-center gap-3"
                      >
                        <item.icon className="h-5 w-5" />
                        <span>{item.label}</span>
                      </Link>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
    </Sidebar>
  );
}
