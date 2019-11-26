$(document).ready(() => {
    const todoTaskController = 'api/TodoTasks';
    const accountController = 'Account';

    const unauthorized = 401;
    const forbidden = 403;
    const notFound = 404;

    let todoTaskComponent = {
        props: ['task', 'idx'],
        template:
            '<div :class="getTaskClass()">' +
                '<div class="todo-task__img col-2">' +
                    '<img :src="getCheckboxImg()" :alt="getCheckboxAlt()" @click="onCheckChange" />' +
                '</div>' +
                '<p class="todo-task__text col-8">{{ task.text }}</p>' +
                '<div class="todo-task__img delete col-2" @click="onDelete">' +
                    '<img src="images/delete.jpg" alt="Delete" />' +
                '</div>' +
            '</div>',
        methods: {
            getTaskClass: function () {
                let defaultClasses = 'todo-task row rounded';

                return this.task.isDone ? defaultClasses + ' done-task' : defaultClasses;
            },
            getCheckboxImg: function () {
                return `images/${this.task.isDone ? 'completed' : 'todo'}.png`;
            },
            getCheckboxAlt: function () {
                return this.task.isDone ? 'done' : 'active';
            },
            onCheckChange: function () {
                this.$emit('oncheckchange', this.task);
            },
            onDelete: function () {
                this.$emit('ondelete', this.task.id, this.idx);
            }
        }
    };

    let waitModalComponent = {
        template:
            '<div class="modal hide fade" role="dialog">' +
                '<div class="modal-dialog" role="document">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header">' +
                            '<h5 class="modal-title">Please wait... :)</h5>' +
                        '</div>' +
                        '<div class="modal-body">' +
                            '<img class="loading-img" src="images/loading.gif" alt="..." />' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>'
    };

    let vm = new Vue({
        el: '#wrapper',
        data: {
            tasks: [],
            newTaskText: '',
            currentList: 'All',
            dataLoaded: false
        },
        components: {
            'todo-task': todoTaskComponent,
            'wait-modal': waitModalComponent
        },
        methods: {
            taskIsEmptyOrSpaces: function () {
                return isEmptyOrSpaces(this.newTaskText);
            },
            onDelete: function (id, idx) {
                deleteTask(id, () => {
                    this.tasks.splice(idx, 1);

                    hideModal();
                });
            },
            onCheckChange: function (task) {
                updateTask(task, () => {
                    task.isDone = !task.isDone;

                    hideModal();
                });
            },
            onAdd: function () {
                if (this.taskIsEmptyOrSpaces()) return;

                addTask(
                    this.newTaskText,
                    todoTask => {
                        this.newTaskText = '';

                        if (this.currentList === 'Active' || this.currentList === 'All') {
                            this.tasks.unshift(todoTask);
                        }

                        hideModal();
                    }
                );
            },
            successLoad: function (data) {
                this.tasks = data;

                hideModal();

                this.dataLoaded = true;
            },
            loadActive: function () {
                loadTasks('Active', data => {
                    this.successLoad(data);
                    this.currentList = 'Active';
                });
            },
            loadDone: function () {
                loadTasks('Done', data => {
                    this.successLoad(data);
                    this.currentList = 'Done';
                });
            },
            loadAll: function () {
                loadTasks('', data => {
                    this.successLoad(data);
                    this.currentList = 'All';
                });
            }
        },
        mounted: function () {
            this.loadAll();
        }
    });

    function isEmptyOrSpaces(str) {
        return str === null || str === '' || str.match(/^ *$/) !== null;
    }
    
    function showError(text) {
        hideModal();
        vm.newTaskText = '';

        let error = $("#error");

        error
            .text(text)
            .show()
            .animate({
                'opacity': '0'
            }, 4000, () => {
                error
                    .css('opacity', '1')
                    .hide();
            });
    }

    function showModal() {
        $('.modal').modal('show');
    }

    function hideModal() {
        $('.modal').modal('hide');
    }

    function errorHandler(error) {
        let message;

        switch (error.status) {
            case unauthorized:
                location.href = `/${accountController}/Login`;
                return;
            case forbidden:
                message = 'You do not have permission to perform this operation';
                break;
            case notFound:
                message = 'Task was not found or server is not responding';
                break;
            default:
                message = 'Error... Status code ' + error.status;
                console.log(error);
                break;
        }

        showError(message);
    }

    function addTask(text, success) {
        showModal();

        $.post({
            url: `/${todoTaskController}/`,
            data: JSON.stringify({ text: text }),
            contentType: 'application/json; charset=utf-8',
            success: success,
            error: errorHandler
        });
    }

    function loadTasks(action, success) {
        showModal();

        $.get({
            url: `/${todoTaskController}/${action}`,
            success: success,
            error: errorHandler
        });
    }

    function deleteTask(id, success) {
        showModal();

        $.ajax({
            url: `/${todoTaskController}/${id}`,
            contentType: 'application/json; charset=utf-8',
            method: 'DELETE',
            crossDomain: true,
            success: success,
            error: errorHandler
        });
    }

    function updateTask(task, success) {
        showModal();

        $.ajax({
            url: `/${todoTaskController}/${task.id}`,
            contentType: 'application/json; charset=utf-8',
            method: 'PUT',
            data: JSON.stringify({
                text: task.text,
                isDone: !task.isDone
            }),
            crossDomain: true,
            success: success,
            error: errorHandler
        });
    }
});