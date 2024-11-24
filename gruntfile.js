module.exports = function (grunt) {
    grunt.initConfig({
        less: {
            dev: {
                options: {
                    paths: ["static/css"]
                },
                files: {
                    "css/compiled.css": "css/import.less"
                }
            }
        },
        watch: {
            files: ['css/**/*.less'],
            tasks: ['less']
        }
    });

    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['less']);
};
