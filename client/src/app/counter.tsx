"use client";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useState } from "react";

export default function Counter() {
  const [counter, setCounter] = useState(0);

  function calculateSum(a: number, b: number) {
    return a + b;
  }

  const isWin = counter >= 10;
  function handleClick() {
    setCounter(counter + 1);
  }

  return (
    <div className="flex flex-col gap-4">
      <CoolCount count={counter} />
      <Button
        onClick={handleClick}
        className="w-fit hover:bg-blue-500 hover:text-white transition-colors"
      >
        Увеличить
      </Button>
      <Input type="text" placeholder="Max Leiter" />
      {isWin && (
        <span>
          Поздравляю Ваня у тебя все получится главное продолжай в том же духе!
        </span>
      )}
    </div>
  );
}
type Props = {
  count: number;
};

function CoolCount({ count }: Props) {
  return <span className="text-#red-500">{count}</span>;
}
