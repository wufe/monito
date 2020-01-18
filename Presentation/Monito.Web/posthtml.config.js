module.exports = {
    plugins: {
        "posthtml-expressions": {
            locals: {
                GA_UA: process.env.GA_UA
            }
        }
    }
};