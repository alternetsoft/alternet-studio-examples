declare var External: any;

(function () {
  type Point2D = { x: number; y: number };

  function sum(arr: number[]): number {
    return arr.reduce((s, v) => s + v, 0);
  }

  function mean(arr: number[]): number {
    if (!arr.length) return NaN;
    return sum(arr) / arr.length;
  }

  function median(arr: number[]): number {
    if (!arr.length) return NaN;
    const copy = arr.slice().sort((a, b) => a - b);
    const mid = Math.floor(copy.length / 2);
    return copy.length % 2 === 0 ? (copy[mid - 1] + copy[mid]) / 2 : copy[mid];
  }

  function variance(arr: number[], sample = false): number {
    if (!arr.length) return NaN;
    const m = mean(arr);
    const sumSq = arr.reduce((s, v) => s + (v - m) ** 2, 0);
    return sumSq / (arr.length - (sample ? 1 : 0));
  }

  function stddev(arr: number[], sample = false): number {
    return Math.sqrt(variance(arr, sample));
  }

  function factorial(n: number): number {
    if (n < 0) return NaN;
    let r = 1;
    for (let i = 2; i <= Math.floor(n); i++) r *= i;
    return r;
  }

  function fibonacci(n: number): number {
    if (n < 0) return NaN;
    let a = 0, b = 1;
    for (let i = 0; i < Math.floor(n); i++) {
      const t = a + b;
      a = b;
      b = t;
    }
    return a;
  }

  function isPrime(n: number): boolean {
    if (n <= 1 || n % 1 !== 0) return false;
    if (n <= 3) return true;
    if (n % 2 === 0) return false;
    const r = Math.floor(Math.sqrt(n));
    for (let i = 3; i <= r; i += 2) if (n % i === 0) return false;
    return true;
  }

function evalExpression(expr: string, vars?: Record<string, number>): number {
  // Whitelist characters roughly (digits, letters, operators, parentheses, spaces, dot)
  if (!/^[0-9a-zA-Z+\-*/%^().,\s]+$/.test(expr)) {
    throw new Error("Expression contains illegal characters");
  }

  const args = vars ? Object.keys(vars) : [];
  const vals = vars ? args.map(k => (vars as Record<string, number>)[k]) : [];

  const fn = new Function(...args, `return (${expr});`);
  // @ts-ignore
  return Number(fn(...(vals as any)));
}

  const Calculator = {
    add: (a: number, b: number) =>
	{ 
		External.DebugWriteLine("Calculator.add is called");
		return a + b;
	},

    sub: (a: number, b: number) => a - b,
    mul: (a: number, b: number) => a * b,
    div: (a: number, b: number) => a / b,
    sum,
    mean,
    median,
    variance,
    stddev,
    factorial,
    fibonacci,
    isPrime,
    evalExpression,
  };

  // Attach to global
  (globalThis as any).Calculator = Calculator;
})();