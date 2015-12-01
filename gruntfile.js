// gruntfile.js
module.exports = function (grunt) {

    grunt.initConfig({
        less: {
            dev: {
                options: {
                    paths: ["static/css"]
                },
                files: {
                    "static/css/compiled.css": "static/css/import.less"
                }
            }
        },
        watch: {
            files: ['<%= less.files %>'],
            tasks: ['less']
        }
    });

    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['less']);

};