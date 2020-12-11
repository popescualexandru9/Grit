
$(document).ready(function () {
    const TIME_LIMIT = 3600;
    const FULL_DASH_ARRAY = 283;
    const ALERT_THRESHOLD = 5;

    let isTimeRunning = false;
    let start_time = 90;
    let timePassed = 0;
    let timeLeft = start_time;
    let timerInterval = null;

    $("#timer").html(`
                    <div class="base-timer">
        <svg class="base-timer__svg" viewBox="0 0 100 100" >
            <g class="base-timer__circle">
                <circle class="base-timer__path-elapsed" cx="50" cy="50" r="45" />
                <path
                    id="base-timer-path-remaining"
                    stroke-dasharray="283"
                    class="base-timer__path-remaining"
                    d="
                                  M 50, 50
                                  m -45, 0
                                  a 45,45 0 1,0 90,0
                                  a 45,45 0 1,0 -90,0">
                </path>
            </g>
        </svg>
        <span id="base-timer-label" class="base-timer__label">
            ${formatTimeLeft(timeLeft)}
        </span>
    </div>`);


    $("#start_button_timer").click(() => {
        if (isTimeRunning === false && start_time <= timePassed) {
            return;
        }

        if (isTimeRunning) {
            stopTimer();
            $("#start_button_timer").html("Start")
            $("#start_button_timer").css("background-color", "#006b00");

        } else {
            startTimer();
            $("#start_button_timer").html("Stop")
            $("#start_button_timer").css("background-color", "#d27000");
        }

        $('#start_button_timer').prop('disabled', true);
        setTimeout(function () {
            $('#start_button_timer').prop('disabled', false);
        }, 1000);
        $("#start_button_timer").blur();
    });

    $("#reset_button_timer").click(() => {
        $(".base-timer__path-remaining").css("color", "#66fcf1");
        $(".base-timer__label").css("color", "#f1f1f1");

        onTimesUp();
        timeLeft = start_time;
        timePassed = 0;
        $("#base-timer-label").html(formatTimeLeft(timeLeft));
        setCircleDasharray();
        setRemainingPathColor(timeLeft);

        $("#start_button_timer").html("Start")
        $("#start_button_timer").css("background-color", "#006b00");

        $("#reset_button_timer").blur();
    });

    $("#add_timer").click(() => {
        if (start_time + 15 > TIME_LIMIT || start_time === timePassed)
            return;
        if (timeLeft < ALERT_THRESHOLD)
            $(".base-timer__label").css("color", "#f1f1f1");


        start_time += 15;
        timeLeft = start_time - timePassed;


        $("#base-timer-label").html(formatTimeLeft(timeLeft));
        setCircleDasharray();
        $("#add_timer").blur();
    });

    $("#sub_timer").click(() => {
        if (start_time - 15 < 1 || timeLeft <= 15)
            return;

        start_time -= 15;
        timeLeft = start_time - timePassed;

        $("#base-timer-label").html(formatTimeLeft(timeLeft));
        setCircleDasharray();
        setRemainingPathColor(timeLeft);
        $("#sub_timer").blur();
    });

    function onTimesUp() {
        clearInterval(timerInterval);
        isTimeRunning = false;
    }

    function startTimer() {

        timerInterval = setInterval(() => {
            isTimeRunning = true;
            $(".base-timer__path-remaining").css("color", "#66fcf1");

            // The amount of time passed increments by one
            timePassed = timePassed += 1;
            timeLeft = start_time - timePassed;

            // The time left label is updated
            $("#base-timer-label").html(formatTimeLeft(timeLeft));

            setCircleDasharray();
            setRemainingPathColor(timeLeft);

            if (timeLeft === 0) {
                onTimesUp();
            }
        }, 1000);
    }

    function stopTimer() {
        onTimesUp();
    }

    function formatTimeLeft(time) {
        let minutes = Math.floor(time / 60);
        let seconds = time % 60;

        if (seconds < 10) {
            seconds = `0${seconds}`;
        }
        if (minutes < 10 && minutes > 0) {
            minutes = `0${minutes}`;
        }
        if (minutes < 1) {
            minutes = `0${minutes}`;
        }


        return `${minutes}:${seconds}`;
    }

    function setCircleDasharray() {
        const circleDasharray = `${(
            calculateTimeFraction() * FULL_DASH_ARRAY
        ).toFixed(0)} 283`;
        $("#base-timer-path-remaining").attr("stroke-dasharray", circleDasharray);
    }

    function calculateTimeFraction() {
        const rawTimeFraction = (timeLeft / start_time);
        return rawTimeFraction - (1 / start_time) * (1 - rawTimeFraction);
    }

    function setRemainingPathColor(timeLeft) {
        if (timeLeft <= ALERT_THRESHOLD) {
            $(".base-timer__path-remaining").css("color", "red");
            $(".base-timer__label").css("color", "red");
        }
    }
});