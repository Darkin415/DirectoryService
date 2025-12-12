import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { SidebarTrigger } from "@/components/ui/sidebar";
import Link from "next/link";

export default function Header() {
  return (
    <header className="sticky top-0 z-10 flex items-center justify-between border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60 px-6 py-4">
      <div className="flex items-center gap-2">
        <SidebarTrigger />
        <span className="text-2xl font-bold text-#011201-600 dark:text-#000000-500">
          <Link href={"/"}>
            <span className="text-2xl font-bold text-green-600 dark:text-green-500">
              DirectoryService{" "}
            </span>
          </Link>
        </span>
      </div>
      <div className="flex items-center gap-4">
        <Avatar>
          <AvatarImage src="https://github.com/shadcn.png" alt="User" />
          <AvatarFallback>DS</AvatarFallback>
        </Avatar>
      </div>
    </header>
  );
}
