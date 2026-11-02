import { useState, useEffect } from "react";
import Button from "./button/Button";
import { PauseIcon, StartIcon } from "~/assets/icons";
import TimerFinishAudio from "~/assets/timer-finish-audio.mp3";

export function CountdownTimer() {
  const [secondsLeft, setSecondsLeft] = useState<number>(1500); // 25 (25 * 60) minutes
  const [paused, setPaused] = useState<boolean>(true);
  const [started, setStarted] = useState<boolean>(false);

  useEffect(() => {
    if (secondsLeft <= 0) {
      const timerFinishAudio = new Audio(TimerFinishAudio);
      timerFinishAudio.play();
      return;
    }
    if (paused) return;

    const intervalId = setInterval(() => {
      if (!paused) setSecondsLeft((prev) => prev - 1);
    }, 1000);

    return () => clearInterval(intervalId);
  }, [secondsLeft, paused]);

  const formatTime = (time: number) => {
    const mins = Math.floor(time / 60)
      .toString()
      .padStart(2, "0");
    const secs = (time % 60).toString().padStart(2, "0");
    return `${mins}:${secs}`;
  };

  const isFinished = secondsLeft <= 0;

  return (
    <div className="flex gap-1 justify-between rounded-2xl border border-gray-200 bg-white p-2 shadow-sm transition-colors dark:border-gray-700 dark:bg-gray-800">
      <Button
        size="sm"
        onClick={() => {
          if (!started) setStarted(true);
          setPaused((prev) => !prev);
        }}
      >
        {paused ? <StartIcon /> : <PauseIcon />}
      </Button>

      <div
        className={`flex items-center justify-center${
          isFinished ? "dark:bg-green-500/10" : "dark:bg-blue-400/5"
        }`}
      >
        <span
          className={`font-mono font-bold tracking-tight tabular-nums ${
            isFinished
              ? "text-green-600 dark:text-green-400"
              : "text-[#1a3a9a] dark:text-blue-400"
          }`}
        >
          {isFinished ? "00:00" : formatTime(secondsLeft)}
        </span>
      </div>
      {!started && (
        <div className="flex gap-1">
          <Button
            variant="outline"
            size="sm"
            onClick={() => setSecondsLeft((prev) => prev + 5 * 60)}
          >
            +
          </Button>
          <Button
            variant="outline"
            size="sm"
            onClick={() => {
              if (secondsLeft === 300) return;
              setSecondsLeft((prev) => prev - 5 * 60);
            }}
          >
            -
          </Button>
        </div>
      )}
    </div>
  );
}
