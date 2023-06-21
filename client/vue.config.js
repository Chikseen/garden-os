module.exports = {
	// ...other vue-cli plugin options...
	pwa: {
		name: "GardenOS",
		themeColor: "#f7f7f7",
		background_color: "#eae3d1",
		msTileColor: "#000000",
		appleMobileWebAppCapable: "yes",
		appleMobileWebAppStatusBarStyle: "black",
		icons: [
			{
				src: "./public/img/icons/apple-touch-icon.png",
				sizes: "196x196",
				type: "image/png",
				purpose: "maskable", // <-- New property value `"maskable"`
			},
		],
	},
};
