(function () {
  function sum(arr) {
    return arr.reduce((s, v) => s + v, 0);
  }

  function mean(arr) {
    if (!arr.length) return NaN;
    return sum(arr) / arr.length;
  }

  function median(arr) {
    if (!arr.length) return NaN;
    var copy = arr.slice().sort(function (a, b) { return a - b; });
    var mid = Math.floor(copy.length / 2);
    return copy.length % 2 === 0 ? (copy[mid - 1] + copy[mid]) / 2 : copy[mid];
  }

  function variance(arr, sample) {
    sample = !!sample;
    if (!arr.length) return NaN;
    var m = mean(arr);
    var sumSq = arr.reduce(function (s, v) { return s + Math.pow(v - m, 2); }, 0);
    return sumSq / (arr.length - (sample ? 1 : 0));
  }

  function stddev(arr, sample) {
    return Math.sqrt(variance(arr, sample));
  }

  function factorial(n) {
    if (n < 0) return NaN;
    var r = 1;
    for (var i = 2; i <= Math.floor(n); i++) r *= i;
    return r;
  }

  function fibonacci(n) {
    if (n < 0) return NaN;
    var a = 0, b = 1;
    for (var i = 0; i < Math.floor(n); i++) {
      var t = a + b;
      a = b;
      b = t;
    }
    return a;
  }

  function isPrime(n) {
    if (n <= 1 || n % 1 !== 0) return false;
    if (n <= 3) return true;
    if (n % 2 === 0) return false;
    var r = Math.floor(Math.sqrt(n));
    for (var i = 3; i <= r; i += 2) if (n % i === 0) return false;
    return true;
  }

function evalExpression(expr, vars) {
  if (!/^[0-9a-zA-Z+\-*/%^().,\s]+$/.test(expr)) {
    throw new Error("Expression contains illegal characters");
  }

  var args = vars ? Object.keys(vars) : [];
  var vals = vars ? args.map(function (k) { return vars[k]; }) : [];

  var fn = Function.apply(null, args.concat("return (" + expr + ");"));
  return Number(fn.apply(null, vals));
}

  var Calculator = {
    add: function (a, b)
    {
       External.DebugWriteLine("Calculator add is called");
       return a + b;
    },
    sub: function (a, b) { return a - b; },
    mul: function (a, b) { return a * b; },
    div: function (a, b) { return a / b; },
    sum: sum,
    mean: mean,
    median: median,
    variance: variance,
    stddev: stddev,
    factorial: factorial,
    fibonacci: fibonacci,
    isPrime: isPrime,
    evalExpression: evalExpression
  };

  this.Calculator = Calculator; // attach to globalThis
})();