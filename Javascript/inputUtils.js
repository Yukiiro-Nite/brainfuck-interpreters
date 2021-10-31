const readline = require('readline')

const getChar = () => {
  process.stdin.setEncoding('utf8')
  if (process.stdin.isTTY) {
    process.stdin.setRawMode(true)
  }

  return new Promise((resolve) => {
    if (process.stdin.readable) {
      resolve(process.stdin.read(1))
    } else {
      process.stdin.on('readable', () => {
        resolve(process.stdin.read(1))
      })
    }
  }).finally(() => {
    if (process.stdin.isTTY) {
      process.stdin.setRawMode(false)
    }
  })
}

const getNumber = () => {
  const rl = readline.createInterface({
    input: process.stdin
  });

  return new Promise((resolve) => {
    rl.on('line', (input) => {
      const number = parseInt(input)
      if (!isNaN(number)) {
        rl.close()
        resolve(number)
      }
    })
  })
}

module.exports = {
  getChar,
  getNumber
}