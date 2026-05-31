(function () {
  let audioContext = null;

  function getAudioContext() {
    const AudioContext = window.AudioContext || window.webkitAudioContext;

    if (!AudioContext) {
      return null;
    }

    if (!audioContext) {
      audioContext = new AudioContext();
    }

    return audioContext;
  }

  // Pokud je AudioContext pozastavený, pokusíme se ho znovu spustit
  async function resumeContext(context) {
    if (!context) {
      return false;
    }

    if (context.state === "suspended") {
      await context.resume();
    }

    return context.state === "running";
  }

  // Funkce dostupné pro Blazor přes FocusTimer
  window.appSound = {
    unlock: async function () {
      try {
        const context = getAudioContext();
        await resumeContext(context);
      } catch {
        // Zvuk není kritický.
      }
    },

    beep: async function () {
      try {
        const context = getAudioContext();

        if (!context) {
          return;
        }

        const canPlay = await resumeContext(context);

        if (!canPlay) {
          return;
        }

        await new Promise(function (resolve) {
          const oscillator = context.createOscillator();
          const gain = context.createGain();

          oscillator.type = "sine";
          oscillator.frequency.value = 880;

          gain.gain.setValueAtTime(0.0001, context.currentTime);
          gain.gain.exponentialRampToValueAtTime(0.12, context.currentTime + 0.02);
          gain.gain.exponentialRampToValueAtTime(0.0001, context.currentTime + 0.28);

          oscillator.connect(gain);
          gain.connect(context.destination);

          oscillator.start();
          oscillator.stop(context.currentTime + 0.3);

          window.setTimeout(resolve, 330);
        });
      } catch {
        // Zvuk není kritický.
      }
    }
  };
})();
