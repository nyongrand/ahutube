const fs = require("fs");
const path = require("path");

const getDirectories = (source) =>
  fs
    .readdirSync(source, { withFileTypes: true })
    .filter((dirent) => dirent.isDirectory())
    .map((dirent) => dirent.name);

const main = "E:\\Videos\\AhuTube\\";
getDirectories(main).forEach((channelFolder) => {
  const jsonFolder = path.join(main, channelFolder, "json");
  const thumbFolder = path.join(main, channelFolder, "thumb");

  if (!fs.existsSync(jsonFolder) || !fs.existsSync(thumbFolder)) return;

  console.log(jsonFolder, thumbFolder);
});
